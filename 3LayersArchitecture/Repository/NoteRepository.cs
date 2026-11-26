
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

        public async Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.NotesModel
                .AsNoTracking()
                .Where(note => note.Id == id)
                .Select(note => new Note(note.Id, note.ItemId, note.Message, note.Color))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Note note, CancellationToken cancellationToken = default)
        {
            var model = new NoteModel()
            {
                Message = note.Message,
                ItemId = note.ItemId,
                Color = note.Color,
                CreatedDate = DateTime.UtcNow
            };

            _context.NotesModel.Add(model);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.NotesModel
                .AsNoTracking()
                .Select(n => new Note(n.Id, n.ItemId, n.Message, n.Color))
                .ToListAsync(cancellationToken);
        }
    }
}
