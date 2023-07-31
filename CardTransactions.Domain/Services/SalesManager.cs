using Amazon.Runtime.Internal;
using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Abstractions;
using CardTransactions.Domain.Documents;
using CardTransactions.Domain.Models;
using ZstdSharp.Unsafe;

namespace CardTransactions.Domain.Services
{
    public class SalesManager : ISalesManager
    {
        private const string SUCCESS_CODE = "00";
        private const string UNSUCCESS_CODE = "-1";

        private readonly IMongoService _mongoService;

        public SalesManager(IMongoService mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task<SalesDocument> GetAsync(string id)
        {
            var list = await _mongoService.ListAsync(new SalesListModel { Id = id });

            return list.FirstOrDefault();
        }

        public async Task<IEnumerable<SalesDocument>> GetListAsync(SalesListModel filter)
        {
            return await _mongoService.ListAsync(filter);
        }

        public async Task SaveAsync(SalesInsertModel request)
        {
            // todo: check..
            var responseCode = GetResponseCodeByLuhnAlgorithm(request.CardNumber);
            var doc = new SalesDocument
            {
                Id = Guid.NewGuid(),
                CardNumber = request.CardNumber,
                ExpiryDate = request.CardEndDate,
                CardFullName = request.CardFullName,
                CardSecurityNumber = request.CardSecurityNumber,
                CreatedUtc = DateTime.UtcNow,
                ResponseCode = responseCode,
                IsSuccess = responseCode == SUCCESS_CODE
            };

            await _mongoService.Add(doc);
        }

        private static string GetResponseCodeByLuhnAlgorithm(string cardNumber)
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

            return result ? SUCCESS_CODE : UNSUCCESS_CODE;
        }
    }
}

