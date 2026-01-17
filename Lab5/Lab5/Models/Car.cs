namespace Lab5.Models;

public class Car : Vehicle
{
    public string Color { get; private set; }

    public Car(string model, int year, string color)
        : base(model, year)
    {
        Color = color;
    }

    public override void ShowInfo()
    {
        Console.WriteLine("CAR");
        base.ShowInfo();
        Console.WriteLine($"Kolor: {Color}");
    }

    public void ChangeColor(string color)
    {
        Color = color;
    }
}
