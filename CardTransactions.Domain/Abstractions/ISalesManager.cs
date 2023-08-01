using CardTransactions.Domain.Documents;
using CardTransactions.Domain.Models;

namespace CardTransactions.Api.Abstractions
{
    /// <summary>
    /// The i sales manager
    /// </summary>
    public interface ISalesManager
    {
        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<SaveReponseModel> SaveAsync(SalesInsertModel request);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<SalesDto> GetAsync(string id);

        /// <summary>
        /// Gets the list asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<IEnumerable<SalesDto>> GetListAsync(SalesListFilter filter);
    }
}

