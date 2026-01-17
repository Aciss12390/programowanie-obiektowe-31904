using Lab5.Models;
using Newtonsoft.Json;

var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");
var data = LoadData(dataPath);

var continueApp = true;

do
{
    Console.WriteLine("Wybierz opcje z menu:");
    Console.WriteLine("1. Lista pojazdow");
    Console.WriteLine("2. Dodaj pojazd");
    Console.WriteLine("3. Usun pojazd");
    Console.WriteLine("4. Zmien kolor pojazdu");
    Console.WriteLine("0. Exit");

    var option = Console.ReadKey().KeyChar;
    Console.WriteLine();

    switch (option)
    {
        case '1':
            ShowCars(data);
            break;
        case '2':
            AddCar(data);
            break;
        case '3':
            RemoveCar(data);
            break;
        case '4':
            ChangeCarColor(data);
            break;
        case '0':
            continueApp = false;
            break;
        default:
            Console.WriteLine("Nieznana opcja");
            break;
    }

    Console.WriteLine();
} while (continueApp);

SaveData(dataPath, data);

static List<Car> LoadData(string path)
{
    if (!File.Exists(path))
    {
        return [];
    }

    var txt = File.ReadAllText(path);
    var data = JsonConvert.DeserializeObject<List<Car>>(txt);
    return data ?? [];
}

static void SaveData(string path, List<Car> data)
{
    var json = JsonConvert.SerializeObject(data, Formatting.Indented);
    File.WriteAllText(path, json);
}

static void ShowCars(List<Car> data)
{
    if (data.Count == 0)
    {
        Console.WriteLine("Brak pojazdow w komisie.");
        return;
    }

    for (var i = 0; i < data.Count; i++)
    {
        Console.WriteLine($"ID: {i}");
        data[i].ShowInfo();
        Console.WriteLine();
    }
}

static void AddCar(List<Car> data)
{
    Console.WriteLine("Podaj model:");
    var model = Console.ReadLine();
    Console.WriteLine("Podaj rok:");
    var success = int.TryParse(Console.ReadLine(), out var year);
    Console.WriteLine("Podaj kolor:");
    var color = Console.ReadLine();

    if (!success || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(color))
    {
        Console.WriteLine("Niepoprawne dane.");
        return;
    }

    data.Add(new Car(model, year, color));
    Console.WriteLine("Dodano pojazd.");
}

static void RemoveCar(List<Car> data)
{
    if (data.Count == 0)
    {
        Console.WriteLine("Brak pojazdow do usuniecia.");
        return;
    }

    Console.WriteLine("Podaj ID pojazdu do usuniecia:");
    var success = int.TryParse(Console.ReadLine(), out var index);

    if (!success || index < 0 || index >= data.Count)
    {
        Console.WriteLine("Niepoprawne ID.");
        return;
    }

    data.RemoveAt(index);
    Console.WriteLine("Usunieto pojazd.");
}

static void ChangeCarColor(List<Car> data)
{
    if (data.Count == 0)
    {
        Console.WriteLine("Brak pojazdow do modyfikacji.");
        return;
    }

    Console.WriteLine("Podaj ID pojazdu do zmiany koloru:");
    var success = int.TryParse(Console.ReadLine(), out var index);

    if (!success || index < 0 || index >= data.Count)
    {
        Console.WriteLine("Niepoprawne ID.");
        return;
    }

    Console.WriteLine("Podaj nowy kolor:");
    var color = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(color))
    {
        Console.WriteLine("Niepoprawny kolor.");
        return;
    }

    data[index].ChangeColor(color);
    Console.WriteLine("Zmieniono kolor.");
}
