using MongoDB.Bson;

namespace CardTransactions.Domain.Documents
{
	public class SalesDocument 
    {
		public SalesDocument()
		{
		}

        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public string CardFullName { get; set; }

        public string ExpiryDate { get; set; }

        public int CardSecurityNumber { get; set; }

        public double Amount { get; set; }

        public DateTime CreatedUtc { get; set; }

        public bool IsSuccess { get; set; }

        public string ResponseCode { get; set; }
    }
}

