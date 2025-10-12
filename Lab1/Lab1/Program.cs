// See https://aka.ms/new-console-template for more information
/*
// Example 1
const int requiredAge = 18;
const string accessDeniedMessage = "You must be 18 years old.";
const string accessAllowedMessage = "Welcome to my shop!";

int age = 16;

while (age < requiredAge) {

Console.Write("Podaj swój wiek: ");
var userInput = Console.ReadLine();

age = Convert.ToInt32(userInput);

if (age < requiredAge)
{
    Console.WriteLine(accessDeniedMessage);
}
else
{
    Console.WriteLine(accessAllowedMessage);
}}
*/
// Example 2
/*
string [] names = {"John", "Jane", "Joe", "Alice", "Artur"};

Console.WriteLine("Names: ");

for (int i = 0; i < names.Length; i++)
{
    Console.WriteLine(names[i]);
}

foreach (string name in names)
{
    Console.WriteLine(name);
}*/


// Zad 1
/*
const string correct_password = "admin123";
string user_password = "admin12";

while (user_password != correct_password)
{
    Console.WriteLine("Podaj hasło: ");
    user_password = Console.ReadLine();
}*/

// Zad 2
/*
int liczba = 0;

do
{
    Console.Write("Podaj liczbę większą od 0: ");
    liczba = Convert.ToInt32(Console.ReadLine());
}
while (liczba <= 0);
Console.WriteLine("Poprawna liczba!");
*/

// Zad 3

/*
string[] cities = { "Toruń", "Wrocław", "Kraków", "Warszawa", "Poznań" };

foreach (string city in cities)
{
    Console.WriteLine($"Miasto: {city}");
}
*/