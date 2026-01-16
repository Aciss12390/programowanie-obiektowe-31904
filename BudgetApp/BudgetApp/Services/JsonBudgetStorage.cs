using System.Text.Json;
using BudzetDomowy.Models;

namespace BudzetDomowy.Services
{
    public class JsonBudgetStorage : IBudgetStorage
    {
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true // żeby plik był czytelny, a nie jedna długa linia
        };

        public void Save(string path, BudgetData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(path, json);
        }

        public BudgetData Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Nie znaleziono pliku JSON.", path);

            string json = File.ReadAllText(path);

            BudgetData? data = JsonSerializer.Deserialize<BudgetData>(json, _options);

            if (data == null)
                throw new InvalidDataException("Plik JSON ma zły format albo jest pusty.");

            return data;
        }
    }
}
