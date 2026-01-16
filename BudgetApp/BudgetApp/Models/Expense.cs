namespace BudzetDomowy.Models
{
    public sealed class Expense : Transaction
    {
        public Expense(int id, DateTime date, decimal amount, string description, int categoryId)
            : base(id, date, amount, description, categoryId)
        {
        }

        public override decimal SignedAmount => -Amount;
    }
}
