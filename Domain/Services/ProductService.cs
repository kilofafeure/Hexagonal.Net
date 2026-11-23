using Domain.Entities;
using Domain.Ports.Primary;
using Domain.Ports.Secondary;

namespace Domain.Services
{
    public class ProductService(IRepository repository) : IService
    {
        private readonly IRepository _repository = repository;

        public void Register(string name, decimal price) {
            var product = new Product(Guid.NewGuid(), name, price);
            _repository.Save(product);
        }

        public IEnumerable<Product> GetAll() {
            return _repository.GetAll();
        }

        public Product? GetById(Guid id) {
            return _repository.GetById(id);
        }
    }
}
