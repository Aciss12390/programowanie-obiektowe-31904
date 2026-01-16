namespace BudzetDomowy.Models
{
    public class LimitWarning
    {
        public int CategoryId { get; private set; }
        public decimal Limit { get; private set; }
        public decimal Spent { get; private set; }

        public decimal OverBy => Spent - Limit;

        public LimitWarning(int categoryId, decimal limit, decimal spent)
        {
            CategoryId = categoryId;
            Limit = limit;
            Spent = spent;
        }
    }
}
