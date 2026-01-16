namespace BudzetDomowy.Models
{
    public abstract class Transaction
    {
        public int Id { get; protected set; }
        public DateTime Date { get; protected set; }
        public decimal Amount { get; protected set; }
        public string Description { get; protected set; }
        public int CategoryId { get; protected set; }

        protected Transaction(int id, DateTime date, decimal amount, string description, int categoryId)
        {
            if (amount <= 0)
                throw new ArgumentException("Kwota musi być większa od 0.");

            Id = id;
            Date = date;
            Amount = amount;
            Description = description?.Trim() ?? "";
            CategoryId = categoryId;
        }

        public abstract decimal SignedAmount { get; }
    }
}
