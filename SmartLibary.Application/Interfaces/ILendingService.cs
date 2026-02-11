using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Application.Interfaces
{
    public interface ILendingService
    {
        Task<bool> RentBookAsync(Guid userId, Guid bookId);
    }
}
