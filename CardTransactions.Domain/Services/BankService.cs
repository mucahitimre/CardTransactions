using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Models;

namespace CardTransactions.Domain.Services
{
    /// <summary>
    /// The bank service
    /// </summary>
    /// <seealso cref="CardTransactions.Api.Abstractions.IBankService" />
    public class BankService : IBankService
    {
        /// <summary>
        /// Pays the specified pay model.
        /// </summary>
        /// <param name="payModel">The pay model.</param>
        /// <returns></returns>
        public async Task<BankPayResponse> Pay(BankPayModel payModel)
        {
            await Task.CompletedTask;

            return new BankPayResponse
            {
                ResponseCode = "00"
            };
        }
    }
}

