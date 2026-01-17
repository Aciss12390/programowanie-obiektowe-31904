namespace Lab5.Models;

public class Bike : Vehicle
{
    public Bike(string model, int year)
        : base(model, year)
    {
    }

    public override void ShowInfo()
    {
        Console.WriteLine("BIKE");
        base.ShowInfo();
    }
}
