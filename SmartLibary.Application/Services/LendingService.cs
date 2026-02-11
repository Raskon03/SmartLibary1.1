using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartLibrary.Application.Interfaces;

namespace SmartLibrary.Application.Services
{
    public class LendingService : ILendingService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IUserRepository _userRepo;

        // Constructor Injection
        public LendingService(IBookRepository bookRepo, IUserRepository userRepo)
        {
            _bookRepo = bookRepo;
            _userRepo = userRepo;
        }

        public async Task<bool> RentBookAsync(Guid userId, Guid bookId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new Exception("Потребителят не е намерен.");

            var book = await _bookRepo.GetByIdAsync(bookId);
            if (book == null) throw new Exception("Книгата не е намерена.");

            if (!book.IsAvailable) return false; // Книгата вече е заета

            // Бизнес логика: променяме статуса
            book.IsAvailable = false;
            await _bookRepo.UpdateAsync(book);

            return true;
        }
    }
}
