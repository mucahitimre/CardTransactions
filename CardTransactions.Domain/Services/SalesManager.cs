using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Abstractions;
using CardTransactions.Domain.Documents;
using CardTransactions.Domain.Models;

namespace CardTransactions.Domain.Services
{
    /// <summary>
    /// The sales manager
    /// </summary>
    /// <seealso cref="CardTransactions.Api.Abstractions.ISalesManager" />
    public class SalesManager : ISalesManager
    {
        private const string SUCCESS_CODE = "00";
        private const string UNSUCCESS_CODE = "-1";

        private readonly IBankService _bankService;
        private readonly IMongoService _mongoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesManager"/> class.
        /// </summary>
        /// <param name="bankService">The bank service.</param>
        /// <param name="mongoService">The mongo service.</param>
        public SalesManager(
            IBankService bankService,
            IMongoService mongoService)
        {
            _bankService = bankService;
            _mongoService = mongoService;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<SalesDto> GetAsync(string id)
        {
            var item = (await _mongoService.ListAsync(new SalesListFilter { Id = id })).FirstOrDefault();
            var dto = DocumentToDto(item);

            return dto;
        }

        /// <summary>
        /// Gets the list asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public async Task<IEnumerable<SalesDto>> GetListAsync(SalesListFilter filter)
        {
            var list = (await _mongoService.ListAsync(filter)).ToList();
            var dtos = new List<SalesDto>();
            foreach (var item in list)
            {
                dtos.Add(DocumentToDto(item));
            }

            return dtos;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<SaveReponseModel> SaveAsync(SalesInsertModel request)
        {
            request.CardNumber = CardNumberValidation(request.CardNumber, out var exception);
            if (exception != null)
            {
                await _mongoService.Add(new SalesDocument
                {
                    Id = Guid.NewGuid(),
                    CardNumber = request.CardNumber,
                    ExpiryDate = request.CardEndDate,
                    CardFullName = request.CardFullName,
                    CardSecurityNumber = request.CardSecurityNumber,
                    Timestamp = DateTime.UtcNow,
                    ResponseCode = UNSUCCESS_CODE,
                    IsSuccess = false,
                    CardType = "Unknown"
                });

                return new SaveReponseModel { ResponseCode = UNSUCCESS_CODE, Message = exception.Message };
            }

            var isValidCard = CheckLuhnAlgorithm(request.CardNumber);
            var responseCode = await PayAndGetResponseCode(request, isValidCard);
            var cardType = GetCardType(request.CardNumber);

            var doc = new SalesDocument
            {
                Id = Guid.NewGuid(),
                CardNumber = request.CardNumber,
                ExpiryDate = request.CardEndDate,
                CardFullName = request.CardFullName,
                CardSecurityNumber = request.CardSecurityNumber,
                Timestamp = DateTime.UtcNow,
                ResponseCode = responseCode,
                Amount = request.Amount,
                IsSuccess = isValidCard,
                CardType = cardType,
            };

            await _mongoService.Add(doc);

            return new SaveReponseModel { ResponseCode = responseCode };
        }

        private static SalesDto DocumentToDto(SalesDocument item)
        {
            return new SalesDto
            {
                Id = item.Id,
                Amount = item.Amount,
                CardType = item.CardType,
                IsSuccess = item.IsSuccess,
                ResponseCode = item.ResponseCode,
                Timestamp = item.Timestamp.ToLocalTime(),
                CardNumber = ConvertToHiddenNumber(item),
            };
        }

        private static string ConvertToHiddenNumber(SalesDocument item)
        {
            return item.CardNumber.Length >= 12 ? item.CardNumber.Substring(0, 6) + "******" + item.CardNumber.Substring(item.CardNumber.Length - 4) : item.CardNumber;
        }

        private async Task<string> PayAndGetResponseCode(SalesInsertModel request, bool isValidCard)
        {
            var responseCode = isValidCard ? SUCCESS_CODE : UNSUCCESS_CODE;
            if (isValidCard)
            {
                var response = await _bankService.Pay(new BankPayModel
                {
                    CardNumber = request.CardNumber,
                    CardFullName = request.CardFullName,
                    CardEndDate = request.CardEndDate,
                    CardSecurityNumber = request.CardSecurityNumber,
                    Amount = request.Amount
                });

                responseCode = response.ResponseCode;
            }

            return responseCode;
        }

        private static string CardNumberValidation(string cardNumber, out Exception exception)
        {
            exception = null;
            if (string.IsNullOrEmpty(cardNumber))
            {
                exception = new Exception("Card number can't be null.");
            }

            cardNumber = cardNumber.Trim().Replace(" ", "");
            var isInvalidCharacter = cardNumber.Trim().ToCharArray().Where(e => e != ' ').ToArray().Any(w => int.TryParse(w.ToString(), out _));
            if (!isInvalidCharacter)
            {
                exception = new Exception("Card number is not available.");
            }

            return cardNumber;
        }

        private static string GetCardType(string cardNumber)
        {
            // Reference: https://en.wikipedia.org/wiki/Payment_card_number
            if (new int[] { 4026, 417500, 4508, 4844, 4913, 4917 }.Any(w => cardNumber.StartsWith(w.ToString())))
            {
                return "Visa Electron";
            }
            else if (cardNumber.StartsWith("4"))
            {
                return "Visa";
            }
            else if (Enumerable.Range(2221, 2720).Any(w => cardNumber.StartsWith(w.ToString())))
            {
                return "Mastercard";
            }
            else if (cardNumber.StartsWith("6011")
                || cardNumber.StartsWith("65")
                || Enumerable.Range(644, 649).Any(w => cardNumber.StartsWith(w.ToString())))
            {
                return "Discover Card";
            }
            else if (Enumerable.Range(3528, 3589).Any(w => cardNumber.StartsWith(w.ToString())))
            {
                return "JCB";
            }

            return "Unknown";
        }

        private static bool CheckLuhnAlgorithm(string cardNumber)
        {
            var items = cardNumber.Trim().ToCharArray().Where(e => e != ' ').ToArray();

            var total = default(int);
            for (int i = default; i < items.Length; i++)
            {
                var item = int.Parse(items[i].ToString());
                if (i % 2 == default)
                {
                    var singular = item * 2;
                    total += singular > 9 ? singular.ToString().ToCharArray().Select(w => int.Parse(w.ToString())).Sum() : singular;
                }
                else
                {
                    total += item;
                }
            }

            var result = total % 10 == default;

            return result;
        }
    }
}

