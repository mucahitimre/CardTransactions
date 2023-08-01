namespace CardTransactions.Domain.Services
{
    /// <summary>
    /// The mongo options
    /// </summary>
    public class MongoOptions
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DbName { get; set; }
    }
}