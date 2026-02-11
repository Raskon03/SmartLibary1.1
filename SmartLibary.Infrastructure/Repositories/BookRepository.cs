using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain;

namespace SmartLibrary.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _books;

        // Изискването: Inject-ваме IOptionsMonitor<MongoDbSettings>
        public BookRepository(IOptionsMonitor<MongoDbSettings> options)
        {
            var mongoClient = new MongoClient(options.CurrentValue.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(options.CurrentValue.DatabaseName);

            _books = mongoDatabase.GetCollection<Book>("Books");
        }

        public async Task CreateAsync(Book book) =>
            await _books.InsertOneAsync(book);

        public async Task<Book?> GetByIdAsync(Guid id) =>
            await _books.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Book>> GetAllAsync() =>
            await _books.Find(_ => true).ToListAsync();

        public async Task UpdateAsync(Book book) =>
            await _books.ReplaceOneAsync(x => x.Id == book.Id, book);
    }
}
