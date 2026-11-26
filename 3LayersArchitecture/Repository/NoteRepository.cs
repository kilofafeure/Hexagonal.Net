
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class NoteRepository(ItemsDbContext context) : IRepository<Note>
    {
        private readonly ItemsDbContext _context = context;

        public async Task AddAsync(Note note, CancellationToken cancellationToken)
        {
            var model = new NoteModel()
            {
                Message = note.Message,
                ItemId = note.ItemId,
                CreatedDate = DateTime.UtcNow
            };

            _context.NotesModel.Add(model);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.NotesModel
                .Select(n => new Note(n.Id, n.ItemId, n.Message))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
