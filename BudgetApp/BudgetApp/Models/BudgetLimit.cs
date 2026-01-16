namespace BudzetDomowy.Models
{
    public class BudgetLimit
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int CategoryId { get; private set; }
        public decimal LimitAmount { get; private set; }

        public BudgetLimit(int year, int month, int categoryId, decimal limitAmount)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Miesiąc musi być od 1 do 12.");

            if (limitAmount <= 0)
                throw new ArgumentException("Limit musi być większy od 0.");

            Year = year;
            Month = month;
            CategoryId = categoryId;
            LimitAmount = limitAmount;
        }
    }
}
