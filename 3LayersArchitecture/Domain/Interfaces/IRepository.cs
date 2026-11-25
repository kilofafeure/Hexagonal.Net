using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Item>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Item item, CancellationToken cancellationToken = default);
    }
}
