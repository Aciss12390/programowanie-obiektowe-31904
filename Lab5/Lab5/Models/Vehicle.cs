namespace Lab5.Models;

public abstract class Vehicle
{
    private readonly string model;
    private readonly int year;

    public string Model => model;
    public int Year => year;

    protected Vehicle(string model, int year)
    {
        this.model = model;
        this.year = year;
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine($"Model: {model}");
        Console.WriteLine($"Rok: {year}");
    }

    public virtual void ShowInfo(string header)
    {
        Console.WriteLine(header);
        ShowInfo();
    }
}
