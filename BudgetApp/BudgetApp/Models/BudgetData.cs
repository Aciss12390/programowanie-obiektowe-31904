namespace BudzetDomowy.Models
{
    // To jest "pudełko", w którym trzymamy wszystko, co chcemy zapisać do pliku.
    public class BudgetData
    {
        public List<Category> Categories { get; set; } = new();
        public List<BudgetLimit> Limits { get; set; } = new();
        public List<TransactionDto> Transactions { get; set; } = new();
    }
}
