using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Factories;
using Repository.Mapping;
using Repository.Model;

namespace Repository
{
    public class NoteRepository(
        ItemsDbContext context,
        IMapper<NoteModel, Note> mapper,
        NoteModelFactory factory) : IRepository<Note>
    {
        private readonly ItemsDbContext _context = context;
        private readonly IMapper<NoteModel, Note> _mapper = mapper;
        private readonly NoteModelFactory _factory = factory;

        public async Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await _context.NotesModel
                .AsNoTracking()
                .FirstOrDefaultAsync(note => note.Id == id, cancellationToken);

            return model is null ? null : _mapper.ToEntity(model);
        }

        public async Task AddAsync(Note note, CancellationToken cancellationToken = default)
        {
            var model = _factory.Create(note);
            _context.NotesModel.Add(model);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var models = await _context.NotesModel
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return models.Select(_mapper.ToEntity).ToList();
        }
    }
}
