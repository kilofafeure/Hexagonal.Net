using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICompleteService
    {
        public Task Complete(int id);
    }
}
