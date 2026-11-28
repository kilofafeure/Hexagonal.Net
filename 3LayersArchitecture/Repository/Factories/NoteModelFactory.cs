using Domain.Entities;
using Repository.Mapping;
using Repository.Model;

namespace Repository.Factories
{
    public sealed class NoteModelFactory(IMapper<NoteModel, Note> mapper)
    {
        public NoteModel Create(Note note)
        {
            var model = mapper.ToModel(note);
            model.DateCreate = DateTime.UtcNow;
            return model;
        }
    }
}
