namespace MultiMappingTests
{
    public class Payment
    {
        public Payment(int invoiceId, int amount, string description)
        {
            InvoiceId = invoiceId;
            Amount = amount;
            Description = description;
        }

        public int InvoiceId { get;  }
        public int Amount { get; }
        public string Description { get;  }
    }
}