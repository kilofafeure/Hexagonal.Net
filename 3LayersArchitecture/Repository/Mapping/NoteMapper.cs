using Domain.Entities;
using Repository.Model;

namespace Repository.Mapping
{
    public sealed class NoteMapper : IMapper<NoteModel, Note>
    {
        public Note ToEntity(NoteModel model)
        {
            return new Note(model.Id, model.ItemId, model.Message, model.Color);
        }

        public NoteModel ToModel(Note entity)
        {
            return new NoteModel
            {
                Id = entity.Id,
                ItemId = entity.ItemId,
                Message = entity.Message,
                Color = entity.Color
            };
        }
    }
}
