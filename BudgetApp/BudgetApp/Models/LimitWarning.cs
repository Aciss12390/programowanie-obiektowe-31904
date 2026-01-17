using System;

namespace BudzetDomowy.Models
{
    public class LimitWarning
    {
        public int CategoryId { get; private set; }
        public string CategoryName { get; private set; }
        public decimal Limit { get; private set; }
        public decimal Spent { get; private set; }

        public decimal OverBy => Spent - Limit;

        public LimitWarning(int categoryId, string categoryName, decimal limit, decimal spent)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Limit = limit;
            Spent = spent;
        }
    }
}
