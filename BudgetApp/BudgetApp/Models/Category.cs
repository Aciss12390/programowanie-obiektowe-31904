namespace BudzetDomowy.Models
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Category(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nazwa kategorii jest wymagana.");

            Id = id;
            Name = name.Trim();
        }
    }
}
