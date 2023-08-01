namespace CardTransactions.Domain.Models
{
    /// <summary>
    /// The bank pay model
    /// </summary>
    public class BankPayModel
    {
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        /// <value>
        /// The card number.
        /// </value>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the full name of the card.
        /// </summary>
        /// <value>
        /// The full name of the card.
        /// </value>
        public string CardFullName { get; set; }

        /// <summary>
        /// Gets or sets the card end date.
        /// </summary>
        /// <value>
        /// The card end date.
        /// </value>
        public string CardEndDate { get; set; }

        /// <summary>
        /// Gets or sets the card security number.
        /// </summary>
        /// <value>
        /// The card security number.
        /// </value>
        public int CardSecurityNumber { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public double Amount { get; set; }
    }
}