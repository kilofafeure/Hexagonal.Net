using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Mapping;
using Repository.Model;

namespace Repository
{
    public class ItemRepository(
        ItemsDbContext context,
        IMapper<ItemModel, Item> mapper) : IRepository<Item>
    {
        private readonly ItemsDbContext _context = context;
        private readonly IMapper<ItemModel, Item> _mapper = mapper;

        public async Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await _context.ItemsModel
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

            return model is null ? null : _mapper.ToEntity(model);
        }

        public async Task AddAsync(Item item, CancellationToken cancellationToken = default)
        {
            var model = _mapper.ToModel(item);
            model.DateCreate = DateTime.UtcNow;

            _context.ItemsModel.Add(model);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Item>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var models = await _context.ItemsModel
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return models.Select(_mapper.ToEntity).ToList();
        }
    }
}
