using ApplicationComponent.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationComponent
{
    public  class ItemService(IRepository repository) : IService
    {
        private readonly IRepository _repository = repository;

        public async Task AddAsync(string title, CancellationToken cancellationToken = default)
        {
            var item = new Item(0, title, false);
            await _repository.AddAsync(item, cancellationToken);
        }

        public async Task<IEnumerable<Item>> GetAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}
