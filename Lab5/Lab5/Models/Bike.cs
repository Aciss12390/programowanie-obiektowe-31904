namespace Lab5.Models;

public class Bike : Vehicle
{
    public Bike(string model, int year, string color)
        : base(model, year, color)
    {
    }

    public override void ShowInfo()
    {
        Console.WriteLine("BIKE");
        base.ShowInfo();
    }
}