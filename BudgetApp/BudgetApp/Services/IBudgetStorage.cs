using BudzetDomowy.Models;

namespace BudzetDomowy.Services
{
    public interface IBudgetStorage
    {
        void Save(string path, BudgetData data);
        BudgetData Load(string path);
    }
}
