using Application.Interfaces;
using Domain.Interfaces;

namespace Application
{
    public class CompleteItemService(ICompleteRepository completeRepository) : ICompleteService
    {
        private readonly ICompleteRepository _completeRepository = completeRepository;

        public Task Complete(int id)
        {
            return _completeRepository.Complete(id);
        }
    }
}
