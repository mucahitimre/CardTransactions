// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Text.Json.Serialization;

namespace CardTransactions.Domain.Models
{
    public class SalesListModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? AmountStart { get; set; }

        public double? AmountEnd { get; set; }

        [JsonIgnore] public string Id { get; set; }
    }
}

