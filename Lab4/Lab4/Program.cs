using System;
using System.Collections.Generic;

namespace Lab4;

class Program
{
    static void Main()
    {
        Console.WriteLine("Mini projekt: Pojazdy");
        MiniProjektPojazdy.Run();

        Console.WriteLine("\nĆwiczenie 1: Polimorfizm");
        PolimorfizmCwiczenie.Run();

        Console.WriteLine("\nĆwiczenie 2: Klasy abstrakcyjne");
        KlasyAbstrakcyjneCwiczenie.Run();

        Console.WriteLine("\nĆwiczenie 3: Interfejsy");
        InterfejsyCwiczenie.Run();

        Console.WriteLine("\nĆwiczenie 4: Mini projekt Zoo");
        ZooCwiczenie.Run();
    }
}

static class MiniProjektPojazdy
{
    public static void Run()
    {
        var pojazd = new Pojazd();
        pojazd.Start();

        var samochod = new Samochod();
        samochod.Start();
        samochod.Jedz();

        var elektryczny = new ElektrycznySamochod();
        elektryczny.Start();
        elektryczny.Jedz();
        elektryczny.Laduj();
    }

    class Pojazd
    {
        public virtual void Start() => Console.WriteLine("Pojazd uruchomiony");
    }

    class Samochod : Pojazd
    {
        public void Jedz() => Console.WriteLine("Samochód jedzie");
    }

    class ElektrycznySamochod : Samochod
    {
        public void Laduj() => Console.WriteLine("Ładowanie baterii...");
    }
}

static class PolimorfizmCwiczenie
{
    public static void Run()
    {
        Zwierze[] zwierzeta = { new Pies(), new Kot(), new Krowa() };

        foreach (Zwierze zwierze in zwierzeta)
        {
            zwierze.DajGlos();
        }
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

    class Krowa : Zwierze
    {
        public override void DajGlos() => Console.WriteLine("Muuu!");
    }
}

static class KlasyAbstrakcyjneCwiczenie
{
    public static void Run()
    {
        List<Pojazd> pojazdy = new()
        {
            new Samochod { Marka = "Toyota" },
            new Motocykl { Marka = "Honda" },
            new Ciezarowka { Marka = "Volvo" }
        };

        foreach (Pojazd pojazd in pojazdy)
        {
            pojazd.Info();
            pojazd.UruchomSilnik();
        }
    }

    abstract class Pojazd
    {
        public string Marka { get; set; } = string.Empty;

        public abstract void UruchomSilnik();

        public void Info()
        {
            Console.WriteLine($"Pojazd marki {Marka}");
        }
    }

    class Samochod : Pojazd
    {
        public override void UruchomSilnik()
        {
            Console.WriteLine("Silnik samochodu został uruchomiony");
        }
    }

    class Motocykl : Pojazd
    {
        public override void UruchomSilnik()
        {
            Console.WriteLine("Motocykl odpala z głośnym dźwiękiem");
        }
    }

    class Ciezarowka : Pojazd
    {
        public override void UruchomSilnik()
        {
            Console.WriteLine("Ciężarówka uruchamia silnik i rusza w trasę");
        }
    }
}

static class InterfejsyCwiczenie
{
    public static void Run()
    {
        IDrukowalne[] dokumenty = { new Dokument(), new Zdjecie() };

        foreach (IDrukowalne element in dokumenty)
        {
            element.Drukuj();
        }
    }

    interface IDrukowalne
    {
        void Drukuj();
    }

    class Dokument : IDrukowalne
    {
        public void Drukuj() => Console.WriteLine("Drukuję dokument...");
    }

    class Zdjecie : IDrukowalne
    {
        public void Drukuj() => Console.WriteLine("Drukuję zdjęcie...");
    }
}

static class ZooCwiczenie
{
    public static void Run()
    {
        List<Zwierze> zwierzeta = new()
        {
            new Lew { Nazwa = "Simba" },
            new Slon { Nazwa = "Dumbo" },
            new Papuga { Nazwa = "Polly" },
            new Tygrys { Nazwa = "Shere Khan" }
        };

        foreach (Zwierze zwierze in zwierzeta)
        {
            Console.WriteLine($"Zwierzę: {zwierze.Nazwa}");
            zwierze.WydajDzwiek();

            if (zwierze is IKarmione karmione)
            {
                karmione.Jedz();
            }

            if (zwierze is ITrenowane trenowane)
            {
                trenowane.Trenuj();
            }

            Console.WriteLine();
        }
    }

    abstract class Zwierze
    {
        public string Nazwa { get; set; } = string.Empty;

        public abstract void WydajDzwiek();
    }

    interface IKarmione
    {
        void Jedz();
    }

    interface ITrenowane
    {
        void Trenuj();
    }

    class Lew : Zwierze, IKarmione, ITrenowane
    {
        public override void WydajDzwiek()
        {
            Console.WriteLine("Lew: Roooar!");
        }

        public void Jedz()
        {
            Console.WriteLine("Lew je mięso.");
        }

        public void Trenuj()
        {
            Console.WriteLine("Lew trenuje skoki przez obręcz.");
        }
    }

    class Slon : Zwierze, IKarmione
    {
        public override void WydajDzwiek()
        {
            Console.WriteLine("Słoń: Truuu!");
        }

        public void Jedz()
        {
            Console.WriteLine("Słoń je trawę.");
        }
    }

    class Papuga : Zwierze, IKarmione, ITrenowane
    {
        public override void WydajDzwiek()
        {
            Console.WriteLine("Papuga: Witaj!");
        }

        public void Jedz()
        {
            Console.WriteLine("Papuga je ziarno.");
        }

        public void Trenuj()
        {
            Console.WriteLine("Papuga trenuje powtarzanie słów.");
        }
    }

    class Tygrys : Zwierze, IKarmione, ITrenowane
    {
        public override void WydajDzwiek()
        {
            Console.WriteLine("Tygrys: Grrr!");
        }

        public void Jedz()
        {
            Console.WriteLine("Tygrys je mięso.");
        }

        public void Trenuj()
        {
            Console.WriteLine("Tygrys trenuje szybki sprint.");
        }
    }
}

//...