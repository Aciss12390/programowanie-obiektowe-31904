using BudzetDomowy.Commands;
using BudzetDomowy.Models;
using BudzetDomowy.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BudzetDomowy.ViewModels
{
    // Dodaję TransactionsViewModel (lista + dodawanie przychodu/wydatku)
    public class TransactionsViewModel : ViewModelBase
    {
        private readonly BudgetManager _budgetManager;

        public ObservableCollection<Transaction> Transactions { get; } = new();

        // Pola formularza
        private DateTime _date = DateTime.Today;
        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); AddCommand.RaiseCanExecuteChanged(); }
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private int _categoryId = 1;
        public int CategoryId
        {
            get => _categoryId;
            set { _categoryId = value; OnPropertyChanged(); }
        }

        private bool _isIncome = true;
        public bool IsIncome
        {
            get => _isIncome;
            set { _isIncome = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCommand { get; }

        public TransactionsViewModel(BudgetManager budgetManager)
        {
            _budgetManager = budgetManager ?? throw new ArgumentNullException(nameof(budgetManager));

            AddCommand = new RelayCommand(AddTransaction, CanAdd);

            Reload();
        }

        private void Reload()
        {
            Transactions.Clear();
            foreach (var t in _budgetManager.GetAll().OrderByDescending(x => x.Date))
                Transactions.Add(t);
        }

        private bool CanAdd()
        {
            return Amount > 0;
        }

        private void AddTransaction()
        {
            // max + 1
            int nextId = _budgetManager.GetAll().Any()
                ? _budgetManager.GetAll().Max(t => t.Id) + 1
                : 1;

            Transaction tx = IsIncome
                ? new Income(nextId, Date, Amount, Description, CategoryId)
                : new Expense(nextId, Date, Amount, Description, CategoryId);

            _budgetManager.Add(tx);

            // odśwież listę
            Reload();

            // wyczyść kwotę po dodaniu
            Amount = 0;
            Description = "";
        }
    }
}
