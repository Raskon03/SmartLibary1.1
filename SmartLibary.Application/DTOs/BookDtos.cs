using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Application.DTOs
{
    public record CreateBookRequest(string Title, string Author);
    public record BookResponse(Guid Id, string Title, string Author, bool IsAvailable);
    public record RentBookRequest(Guid UserId, Guid BookId);
}
