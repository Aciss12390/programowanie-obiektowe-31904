namespace BudzetDomowy.Models
{
    // DTO = prosta wersja obiektu "do przenoszenia", np. do pliku.
    public class TransactionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = "";
        public int CategoryId { get; set; }

        // "Income" albo "Expense" - dzięki temu wiemy co złożyć po wczytaniu.
        public string Type { get; set; } = "";
    }
}
