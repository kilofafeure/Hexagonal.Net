using Domain.Entities;
using Repository.Model;

namespace Repository.Mapping
{
    public sealed class ItemMapper : IMapper<ItemModel, Item>
    {
        public Item ToEntity(ItemModel model)
        {
            return new Item(model.Id, model.Title, model.IsCompleted);
        }

        public ItemModel ToModel(Item entity)
        {
            return new ItemModel
            {
                Id = entity.Id,
                Title = entity.Title,
                IsCompleted = entity.IsCompleted
            };
        }
    }
}
