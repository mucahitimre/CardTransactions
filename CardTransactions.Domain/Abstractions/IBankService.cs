using CardTransactions.Domain.Models;

namespace CardTransactions.Api.Abstractions
{
    /// <summary>
    /// The i bank service
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// Pays the specified pay model.
        /// </summary>
        /// <param name="payModel">The pay model.</param>
        /// <returns></returns>
        Task<BankPayResponse> Pay(BankPayModel payModel);
    }
}

