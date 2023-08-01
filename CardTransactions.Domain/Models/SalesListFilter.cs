// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Text.Json.Serialization;

namespace CardTransactions.Domain.Models
{
    /// <summary>
    /// The sales list filter
    /// </summary>
    public class SalesListFilter
    {
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        /// <example>2023-08-01 09:45:22</example>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        /// <example>2023-08-01 09:45:22</example>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the amount start.
        /// </summary>
        /// <value>
        /// The amount start.
        /// </value>
        public double? AmountStart { get; set; }

        /// <summary>
        /// Gets or sets the amount end.
        /// </summary>
        /// <value>
        /// The amount end.
        /// </value>
        public double? AmountEnd { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonIgnore] public string Id { get; set; }
    }
}

