using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartLibrary.Domain;

namespace SmartLibrary.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);
        Task CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task<List<Book>> GetAllAsync();
    }
}
