using CardTransactions.Domain.Documents;
using CardTransactions.Domain.Models;

namespace CardTransactions.Domain.Abstractions
{
    public interface IMongoService
    {
        void CreateIndex();

        Task Add(SalesDocument document);

        Task<IEnumerable<SalesDocument>> ListAsync(SalesListModel filter);
    }
}

