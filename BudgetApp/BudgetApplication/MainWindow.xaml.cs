using System.Windows;
using BudzetDomowy.Services;
using BudzetDomowy.ViewModels;

namespace BudgetApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var manager = new BudgetManager();
            DataContext = new TransactionsViewModel(manager);
        }
    }
}
