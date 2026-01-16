using BudzetDomowy.Models;

namespace BudzetDomowy.Services
{
    // Interfejs = umowa: "kto to implementuje, musi umieć Save i Load"
    public interface IBudgetStorage
    {
        void Save(string path, BudgetData data);
        BudgetData Load(string path);
    }
}
