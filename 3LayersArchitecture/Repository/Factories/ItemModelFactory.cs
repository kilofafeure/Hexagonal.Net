using Domain.Entities;
using Repository.Mapping;
using Repository.Model;

namespace Repository.Factories
{
    public sealed class ItemModelFactory(IMapper<ItemModel, Item> mapper)
    {
        public ItemModel Create(Item item)
        {
            var model = mapper.ToModel(item);
            model.DateCreate = DateTime.UtcNow;
            return model;
        }
    }
}
