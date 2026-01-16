using BudzetDomowy.Models;
using System.Linq;


namespace BudzetDomowy.Services
{
    public class BudgetManager
    {
        private readonly List<Transaction> _transactions = new();
        private readonly List<BudgetLimit> _limits = new();


        public void Add(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            _transactions.Add(transaction);
        }

        public IReadOnlyList<Transaction> GetAll()
        {
            return _transactions;
        }

        public decimal GetBalance()
        {
            decimal total = 0;

            foreach (var t in _transactions)
                total += t.SignedAmount;

            return total;
        }

        public IReadOnlyList<Transaction> GetByMonth(int year, int month)
        {
            // wybieramy tylko te transakcje, które są z danego roku i miesiąca
            return _transactions
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .ToList();
        }

        public decimal GetIncomeSum(int year, int month)
        {
            // bierzemy tylko przychody z miesiąca i sumujemy
            return _transactions
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .OfType<Income>()
                .Sum(t => t.Amount);
        }

        public decimal GetExpenseSum(int year, int month)
        {
            // bierzemy tylko wydatki z miesiąca i sumujemy
            return _transactions
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .OfType<Expense>()
                .Sum(t => t.Amount);
        }

        public Dictionary<int, decimal> GetExpenseSumsByCategory(int year, int month)
        {
            return _transactions
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .OfType<Expense>()
                .GroupBy(t => t.CategoryId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(x => x.Amount)
                );
        }

        public void SetLimit(BudgetLimit limit)
        {
            if (limit == null)
                throw new ArgumentNullException(nameof(limit));

            // usuwamy stary limit dla tego samego roku/miesiąca/kategorii (żeby nie było duplikatów)
            _limits.RemoveAll(l =>
                l.Year == limit.Year &&
                l.Month == limit.Month &&
                l.CategoryId == limit.CategoryId);

            _limits.Add(limit);
        }

        public IReadOnlyList<LimitWarning> GetLimitWarnings(int year, int month)
        {
            var warnings = new List<LimitWarning>();

            // ile wydano na kategorie w tym miesiącu
            var spentByCategory = GetExpenseSumsByCategory(year, month);

            // bierzemy limity tylko z danego miesiąca
            var monthLimits = _limits.Where(l => l.Year == year && l.Month == month);

            foreach (var limit in monthLimits)
            {
                // jeśli w ogóle nic nie wydano w tej kategorii, to traktujemy jako 0
                decimal spent = 0;
                if (spentByCategory.TryGetValue(limit.CategoryId, out var value))
                    spent = value;

                // jeśli wydano więcej niż limit -> tworzymy ostrzeżenie
                if (spent > limit.LimitAmount)
                    warnings.Add(new LimitWarning(limit.CategoryId, limit.LimitAmount, spent));
            }

            return warnings;
        }

        public IReadOnlyList<BudgetLimit> GetLimits()
        {
            return _limits;
        }



    }
}
