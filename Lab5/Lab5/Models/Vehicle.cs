namespace Lab5.Models;

public abstract class Vehicle
{
    private string model;
    private int year;
    private string color;

    protected Vehicle(string model, int year, string color)
    {
        this.model = model;
        this.year = year;
        this.color = color;
    }

    public string Model
    {
        get { return model; }
    }

    public int Year
    {
        get { return year; }
    }

    public string Color
    {
        get { return color; }
        set { color = value; }
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine($"Model: {model}");
        Console.WriteLine($"Rok: {year}");
        Console.WriteLine($"Kolor: {color}");
    }

    public virtual void ShowInfo(string header)
    {
        Console.WriteLine(header);
        ShowInfo();
    }
}