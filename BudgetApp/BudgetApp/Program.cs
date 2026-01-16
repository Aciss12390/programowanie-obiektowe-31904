using BudzetDomowy.Models;
using BudzetDomowy.Services;

var manager = new BudgetManager();

var categories = new List<Category>
{
    new Category(1, "Praca"),
    new Category(2, "Transport"),
    new Category(3, "Jedzenie")
};

manager.Add(new Income(1, DateTime.Today, 4000m, "Wypłata", 1));
manager.Add(new Expense(2, DateTime.Today, 120m, "Zakupy spożywcze", 3));
manager.Add(new Expense(3, DateTime.Today, 60m, "Paliwo", 2));
manager.Add(new Expense(4, DateTime.Today, 200m, "Naprawa auta", 2));

int year = DateTime.Today.Year;
int month = DateTime.Today.Month;

manager.SetLimit(new BudgetLimit(year, month, 2, 200m));
manager.SetLimit(new BudgetLimit(year, month, 3, 150m));

// Składamy dane do zapisu
var dataToSave = new BudgetData
{
    Categories = categories,
    Limits = manager.GetLimits().ToList(),
    Transactions = manager.GetAll().Select(t => new TransactionDto
    {
        Id = t.Id,
        Date = t.Date,
        Amount = t.Amount,
        Description = t.Description,
        CategoryId = t.CategoryId,
        Type = t is Income ? "Income" : "Expense"
    }).ToList()
};

// Zapis do SQLite
IBudgetStorage storage = new SqliteBudgetStorage();
string dbPath = "budget.db";
storage.Save(dbPath, dataToSave);

Console.WriteLine("Zapisano do bazy:");
Console.WriteLine(Path.GetFullPath(dbPath));

// Odczyt z SQLite
var loaded = storage.Load(dbPath);

// Składamy managera od nowa, jakby aplikacja odpaliła świeżo
var manager2 = new BudgetManager();

foreach (var limit in loaded.Limits)
    manager2.SetLimit(limit);

foreach (var dto in loaded.Transactions)
{
    if (dto.Type == "Income")
        manager2.Add(new Income(dto.Id, dto.Date, dto.Amount, dto.Description, dto.CategoryId));
    else
        manager2.Add(new Expense(dto.Id, dto.Date, dto.Amount, dto.Description, dto.CategoryId));
}

Console.WriteLine("\nSaldo po wczytaniu z SQLite:");
Console.WriteLine(manager2.GetBalance());

var categoryNames = loaded.Categories.ToDictionary(c => c.Id, c => c.Name);
var warnings = manager2.GetLimitWarnings(year, month);

Console.WriteLine("\nOstrzeżenia po wczytaniu:");
if (warnings.Count == 0)
{
    Console.WriteLine("Brak przekroczonych limitów.");
}
else
{
    foreach (var w in warnings)
    {
        var name = categoryNames.TryGetValue(w.CategoryId, out var n) ? n : $"Kategoria {w.CategoryId}";
        Console.WriteLine($"{name}: limit {w.Limit}, wydano {w.Spent}, przekroczono o {w.OverBy}");
    }
}
