using Domain.Entities;

namespace API.Contracts;

public sealed record ItemResponse(
    int Id,
    string Title,
    bool IsCompleted)
{
    public static ItemResponse FromDomain(Item item)
    {
        return new ItemResponse(item.Id, item.Title, item.IsCompleted);
    }
}
