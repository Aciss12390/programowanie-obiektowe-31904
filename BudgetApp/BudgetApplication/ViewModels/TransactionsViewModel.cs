using BudzetDomowy.Commands;
using BudzetDomowy.Models;
using BudzetDomowy.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace BudzetDomowy.ViewModels
{
    // Dodaję TransactionsViewModel (lista + dodawanie przychodu/wydatku)
    public class TransactionsViewModel : ViewModelBase
    {
        private readonly BudgetManager _budgetManager;

        public ObservableCollection<Transaction> Transactions { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<LimitWarning> Warnings { get; } = new();

        private string _statusMessage = "";
        public string StatusMessage
        {
            get => _statusMessage;
            private set { _statusMessage = value; OnPropertyChanged(); }
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            private set { _balance = value; OnPropertyChanged(); }
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                AddLimitCommand.RaiseCanExecuteChanged();
            }
        }

        // Pola formularza
        private DateTime? _date = DateTime.Today;
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
                UpdateWarnings();
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private bool _isIncome = true;
        public bool IsIncome
        {
            get => _isIncome;
            set { _isIncome = value; OnPropertyChanged(); }
        }

        private decimal _limitAmount;
        public decimal LimitAmount
        {
            get => _limitAmount;
            set
            {
                _limitAmount = value;
                OnPropertyChanged();
                AddLimitCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand AddLimitCommand { get; }
        public RelayCommand SaveJsonCommand { get; }
        public RelayCommand LoadJsonCommand { get; }

        public TransactionsViewModel(BudgetManager budgetManager)
        {
            _budgetManager = budgetManager ?? throw new ArgumentNullException(nameof(budgetManager));

            AddCommand = new RelayCommand(AddTransaction, CanAdd);
            AddLimitCommand = new RelayCommand(AddLimit, CanAddLimit);
            SaveJsonCommand = new RelayCommand(SaveJson);
            LoadJsonCommand = new RelayCommand(LoadJson);

            Reload();
        }

        private void Reload()
        {
            ReloadCategories();

            Transactions.Clear();
            foreach (var t in _budgetManager.GetAll().OrderByDescending(x => x.Date))
                Transactions.Add(t);

            Balance = _budgetManager.GetBalance();
            UpdateWarnings();
        }

        private void ReloadCategories()
        {
            int? selectedId = SelectedCategory?.Id;

            Categories.Clear();
            foreach (var c in _budgetManager.GetCategories())
                Categories.Add(c);

            SelectedCategory = Categories.FirstOrDefault(c => c.Id == selectedId) ?? Categories.FirstOrDefault();
        }

        private bool CanAdd()
        {
            return Amount > 0;
        }

        private void AddTransaction()
        {
            // max + 1 (GetAll tylko raz)
            var all = _budgetManager.GetAll();
            int nextId = all.Any() ? all.Max(t => t.Id) + 1 : 1;

            int categoryId = SelectedCategory?.Id ?? 1;
            DateTime date = Date ?? DateTime.Today;

            Transaction tx = IsIncome
                ? new Income(nextId, date, Amount, Description, categoryId)
                : new Expense(nextId, date, Amount, Description, categoryId);

            _budgetManager.Add(tx);

            Reload();

            // wyczyść pola po dodaniu
            Amount = 0;
            Description = "";
        }

        private bool CanAddLimit()
        {
            return LimitAmount > 0 && SelectedCategory != null;
        }

        private void AddLimit()
        {
            var d = Date ?? DateTime.Today;
            int categoryId = SelectedCategory?.Id ?? 1;

            var limit = new BudgetLimit(d.Year, d.Month, categoryId, LimitAmount);

            _budgetManager.AddOrUpdateLimit(limit);

            LimitAmount = 0;

            UpdateWarnings();
        }

        private void SaveJson()
        {
            try
            {
                string path = GetJsonPath();
                _budgetManager.SaveToJson(path);
                StatusMessage = $"Zapisano dane do pliku: {path}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Błąd zapisu: {ex.Message}";
            }
        }

        private void LoadJson()
        {
            try
            {
                string path = GetJsonPath();
                _budgetManager.LoadFromJson(path);
                Reload();
                StatusMessage = $"Wczytano dane z pliku: {path}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Błąd wczytywania: {ex.Message}";
            }
        }

        private static string GetJsonPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "budget.json");
        }

        private void UpdateWarnings()
        {
            Warnings.Clear();

            var d = Date ?? DateTime.Today;
            var warnings = _budgetManager.GetLimitWarnings(d.Year, d.Month);

            foreach (var w in warnings)
                Warnings.Add(w);
        }
    }
}
