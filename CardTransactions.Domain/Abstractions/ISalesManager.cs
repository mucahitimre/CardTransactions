using CardTransactions.Domain.Documents;
using CardTransactions.Domain.Models;

namespace CardTransactions.Api.Abstractions
{
    public interface ISalesManager
    {
        Task SaveAsync(SalesInsertModel request);

        Task<SalesDocument> GetAsync(string id);

        Task<IEnumerable<SalesDocument>> GetListAsync(SalesListModel filter);
    }
}

