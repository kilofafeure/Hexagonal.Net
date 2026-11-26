using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class ItemRepository(ItemsDbContext context) : IRepository<Item>
    {
        private readonly ItemsDbContext _context = context;

        public async Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ItemsModel
                .AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new Item(item.Id, item.Title, item.IsCompleted))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Item item, CancellationToken cancellationToken = default)
        {
            var model = new ItemModel()
            {
                Title = item.Title,
                IsCompleted = item.IsCompleted,
                CreatedDate = DateTime.UtcNow,
            };

            _context.ItemsModel.Add(model);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Item>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.ItemsModel
                .AsNoTracking()
                .Select(e => new Item(e.Id, e.Title, e.IsCompleted))
                .ToListAsync(cancellationToken);
        }
    }
}
