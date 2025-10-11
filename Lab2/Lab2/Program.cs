
// See https://aka.ms/new-console-template for more information
/*
Car car1 = new Car("VM", 2025);
Car car2 = new Car("Audi", 2015);
Car car3 = new Car("BMW", 2020);

car1.Show();
car2.Show();
car3.Show();

Console.WriteLine("Your car: {0}", car1.Model);

class Car
{
    private string model;
    private int year;

    public Car(string model, int year)
    {
        this.model = model;
        this.year = year;
    }

    public string Model
    {
        get { return model; }
    }
    
    public int Year
    {
        get { return year; }
    }

    public void Show()
    {
        Console.WriteLine("Model: {0}", "Year: {0}", model, year);
    }
}*/

Osoba osoba1 = new Osoba("Ania", 18);
Osoba osoba2 = new Osoba("Ola", 4);

osoba1.PrzedstawSie();
osoba2.PrzedstawSie();

class Osoba
{
    private string Imie;
    private int Wiek;

    public void PrzedstawSie()
    {
        Console.WriteLine($"Mam na imię: {Imie}, wiek: {Wiek}");
    }

    public Osoba(string imie, int wiek)
    {
        Imie = imie;
        Wiek = wiek;
    }
}