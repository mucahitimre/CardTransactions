namespace CardTransactions.Domain.Models
{
    public class SalesInsertModel
    {
        public string CardNumber { get; set; }

        public string CardFullName { get; set; }

        public string CardEndDate { get; set; }

        public int CardSecurityNumber { get; set; }

        public double Price { get; set; }
    }
}