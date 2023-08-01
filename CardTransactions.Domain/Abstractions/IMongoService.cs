using CardTransactions.Domain.Documents;
using CardTransactions.Domain.Models;

namespace CardTransactions.Domain.Abstractions
{
    /// <summary>
    /// The i mongo service
    /// </summary>
    public interface IMongoService
    {
        /// <summary>
        /// Creates the index.
        /// </summary>
        void CreateIndexAsync();

        /// <summary>
        /// Adds the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        Task Add(SalesDocument document);

        /// <summary>
        /// Lists the asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<IEnumerable<SalesDocument>> ListAsync(SalesListFilter filter);

        /// <summary>
        /// Creates the dumy date asynchronous.
        /// </summary>
        /// <returns></returns>
        Task CreateDumyDateAsync();
    }
}

