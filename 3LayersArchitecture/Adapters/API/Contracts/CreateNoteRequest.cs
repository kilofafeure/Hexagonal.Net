namespace API.Contracts;

public sealed record CreateNoteRequest(string Message, string? Color = null);
