using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class ItemRepository(ItemsDbContext context) : IRepository
    {
        private readonly ItemsDbContext _context = context;

        public async Task AddAsync(Item item)
        {
            var model = new ItemModel()
            {
                Title = item.Title,
                IsCompleted = item.IsCompleted,
                CreatedDate = DateTime.Now,
            };

            _context.ItemsModel.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.ItemsModel
                .Select(e => new Item(e.Id, e.Title, e.IsCompleted))
                .ToListAsync();
        }
    }
}
