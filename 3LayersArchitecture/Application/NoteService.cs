using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public class NoteService(IRepository<Note> repository) : IService<Note>
    {
        private readonly IRepository<Note> _repository = repository;

        public async Task AddAsync(Note entity, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<Note>> GetAsync(CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}
