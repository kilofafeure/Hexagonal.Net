namespace Domain.Entities
{
    public class Product
    {
        private string _name = string.Empty;
        private decimal _price;

        public Guid Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) {
                    throw new ArgumentNullException(nameof(value), "Name can't be empty");
                }

                _name = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentNullException(nameof(value), "Price must be greater than 0");
                }

                _price = value;
            }
        }

        public Product(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
