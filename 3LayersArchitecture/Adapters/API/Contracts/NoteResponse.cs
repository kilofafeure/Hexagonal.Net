using Domain.Entities;

namespace API.Contracts;

public sealed record NoteResponse(
    int Id,
    int ItemId,
    string Message,
    string? Color)
{
    public static NoteResponse FromDomain(Note note)
    {
        return new NoteResponse(note.Id, note.ItemId, note.Message, note.Color);
    }
}
