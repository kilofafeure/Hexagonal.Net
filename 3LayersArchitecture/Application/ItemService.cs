using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public  class ItemService(IRepository<Item> repository) : IService<Item>
    {
        private readonly IRepository<Item> _repository = repository;

        public Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _repository.GetByIdAsync(id, cancellationToken);
        }

        public async Task AddAsync(Item item, CancellationToken cancellationToken = default)
        {
            await _repository.AddAsync(item, cancellationToken);
        }

        public async Task<IEnumerable<Item>> GetAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}
