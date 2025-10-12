// See https://aka.ms/new-console-template for more information

// Zad 5

/*

KontoBankowe Santander = new KontoBankowe();
Santander.Wplata(2000);

Console.WriteLine("Saldo konta: " + Santander.PobierzSaldo());

Santander.Wyplata(500);

Console.WriteLine("Saldo konta po wypłacie: " + Santander.PobierzSaldo());

class KontoBankowe
{
    private double saldo;
    public void Wplata(double kwota) { saldo += kwota; }
    public double PobierzSaldo() { return saldo; }

    public void Wyplata(double kwota)
    {
        if (kwota > saldo)
        {
            Console.WriteLine("Niewystarczajace srodki na koncie.");
        }
        else
        {
            saldo -= kwota;
            Console.WriteLine("Wypłacono: " + kwota);
        }
    }
}

*/

//Zad 6

/*

Kot kot = new Kot();
kot.Miaucz();

class Zwierze
{
    public void Jedz() => Console.WriteLine("Zwierzę je");
}
class Pies : Zwierze
{
    public void Szczekaj() => Console.WriteLine("Hau hau!");
}

class Kot : Zwierze
{
    public void Miaucz() => Console.WriteLine("Miau miau!");
}

*/

//Zad 7

/*

Zwierze[] zwierzeta = { new Pies(), new Kot() };

foreach (var zwierze in zwierzeta)
{
    zwierze.DajGlos();
}

class Zwierze
{
    public virtual void DajGlos() => Console.WriteLine("Zwierzę wydaje dźwięk");
}
class Pies : Zwierze
{
    public override void DajGlos() => Console.WriteLine("Hau hau!");
}
class Kot : Zwierze
{
    public override void DajGlos() => Console.WriteLine("Miau!");
}

*/

