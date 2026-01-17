using BudzetDomowy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudzetDomowy.Services
{
    public class BudgetManager
    {
        private const string IncomeType = "Income";
        private const string ExpenseType = "Expense";

        private readonly List<Transaction> _transactions = new();
        private readonly List<BudgetLimit> _limits = new();
        private readonly List<Category> _categories = new();

        public BudgetManager()
        {
            // Domyślne kategorie
            _categories.AddRange(CreateDefaultCategories());
        }

        public IReadOnlyList<Category> GetCategories()
        {
            return _categories;
        }

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
            // wybieram tylko te transakcje, które są z danego roku i miesiąca
            return _transactions
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .ToList();
        }

        public decimal GetIncomeSum(int year, int month)
        {
            // biorę tylko przychody z miesiąca i sumuję
            return _transactions
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .OfType<Income>()
                .Sum(t => t.Amount);
        }

        public decimal GetExpenseSum(int year, int month)
        {
            // biorę tylko wydatki z miesiąca i sumuję
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
            AddOrUpdateLimit(limit);
        }

        public void AddOrUpdateLimit(BudgetLimit limit)
        {
            if (limit == null)
                throw new ArgumentNullException(nameof(limit));

            var existing = _limits.FirstOrDefault(l =>
                l.Year == limit.Year &&
                l.Month == limit.Month &&
                l.CategoryId == limit.CategoryId);

            if (existing != null)
                _limits.Remove(existing);

            _limits.Add(limit);
        }

        public IReadOnlyList<LimitWarning> GetLimitWarnings(int year, int month)
        {
            var warnings = new List<LimitWarning>();

            // ile wydano na kategorie w tym miesiącu
            var spentByCategory = GetExpenseSumsByCategory(year, month);

            // biorę limity tylko z danego miesiąca
            var monthLimits = _limits.Where(l => l.Year == year && l.Month == month);

            foreach (var limit in monthLimits)
            {
                // jeśli w ogóle nic nie wydano w tej kategorii, to traktuję jako 0
                decimal spent = 0;
                if (spentByCategory.TryGetValue(limit.CategoryId, out var value))
                    spent = value;

                // jeśli wydano więcej niż limit -> tworzę ostrzeżenie
                if (spent > limit.LimitAmount)
                {
                    var category = _categories.FirstOrDefault(c => c.Id == limit.CategoryId);
                    string categoryName = category?.Name ?? $"Kategoria {limit.CategoryId}";

                    warnings.Add(new LimitWarning(limit.CategoryId, categoryName, limit.LimitAmount, spent));
                }
            }

            return warnings;
        }

        public IReadOnlyList<BudgetLimit> GetLimits()
        {
            return _limits;
        }

        public void SaveToJson(string path)
        {
            var storage = new JsonBudgetStorage();
            storage.Save(path, ExportData());
        }

        public void LoadFromJson(string path)
        {
            var storage = new JsonBudgetStorage();
            var data = storage.Load(path);
            ImportData(data);
        }

        public void SaveToSqlite(string path)
        {
            var storage = new SqliteBudgetStorage();
            storage.Save(path, ExportData());
        }

        public void LoadFromSqlite(string path)
        {
            var storage = new SqliteBudgetStorage();
            var data = storage.Load(path);
            ImportData(data);
        }

        public BudgetData ExportData()
        {
            var data = new BudgetData();

            data.Categories.AddRange(_categories);
            data.Limits.AddRange(_limits);

            foreach (var t in _transactions)
            {
                data.Transactions.Add(new TransactionDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Description = t.Description,
                    CategoryId = t.CategoryId,
                    Type = t is Income ? IncomeType : ExpenseType
                });
            }

            return data;
        }

        public void ImportData(BudgetData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _categories.Clear();
            if (data.Categories.Count > 0)
            {
                foreach (var c in data.Categories)
                    _categories.Add(new Category(c.Id, c.Name));
            }
            else
            {
                _categories.AddRange(CreateDefaultCategories());
            }

            _limits.Clear();
            foreach (var l in data.Limits)
                _limits.Add(new BudgetLimit(l.Year, l.Month, l.CategoryId, l.LimitAmount));

            _transactions.Clear();
            foreach (var t in data.Transactions)
            {
                var description = t.Description ?? "";
                Transaction transaction = t.Type switch
                {
                    IncomeType => new Income(t.Id, t.Date, t.Amount, description, t.CategoryId),
                    ExpenseType => new Expense(t.Id, t.Date, t.Amount, description, t.CategoryId),
                    _ => throw new InvalidDataException($"Nieznany typ transakcji: {t.Type}")
                };

                _transactions.Add(transaction);
            }
        }

        private static List<Category> CreateDefaultCategories()
        {
            return new List<Category>
            {
                new Category(1, "Jedzenie"),
                new Category(2, "Transport"),
                new Category(3, "Rachunki"),
                new Category(4, "Rozrywka"),
                new Category(5, "Inne")
            };
        }
    }
}
