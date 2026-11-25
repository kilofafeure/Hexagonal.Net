using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationComponent.Interfaces
{
    public interface IService
    {
        Task<IEnumerable<Item>> GetAsync();
        Task AddAsync(string title);
    }
}
