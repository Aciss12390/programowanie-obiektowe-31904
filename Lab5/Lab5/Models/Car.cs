namespace Lab5.Models;

public class Car : Vehicle
{
    public Car(string model, int year, string color)
        : base(model, year, color)
    {
    }

    public override void ShowInfo()
    {
        Console.WriteLine("CAR");
        base.ShowInfo();
    }
}