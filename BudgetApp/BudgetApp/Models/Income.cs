namespace BudzetDomowy.Models
{
    public sealed class Income : Transaction
    {
        public Income(int id, DateTime date, decimal amount, string description, int categoryId)
            : base(id, date, amount, description, categoryId)
        {
        }

        public override decimal SignedAmount => Amount;
    }
}
