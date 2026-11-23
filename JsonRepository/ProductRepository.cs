using Domain.Entities;
using Domain.Ports.Secondary;
using System.Text.Json;

namespace JsonRepository
{
    public class ProductRepository(string path) : IRepository
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true
        };

        private readonly string _path = path;

        public void Save(Product product)
        {
            var products = GetAll();
            products.Add(product);
            string jsonString = JsonSerializer.Serialize(products, SerializerOptions);
            File.WriteAllText(_path, jsonString);
        }

        public List<Product> GetAll()
        {
            if (!File.Exists(_path))
                return [];

            string jsonString = File.ReadAllText(_path);
            var products = JsonSerializer.Deserialize<List<Product>>(jsonString);
            return products ?? [];
        }
    }
}
