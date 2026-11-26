using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IService
    {
        Task<IEnumerable<Item>> GetAsync(CancellationToken cancellationToken = default);
        Task AddAsync(string title, CancellationToken cancellationToken = default);
    }
}
