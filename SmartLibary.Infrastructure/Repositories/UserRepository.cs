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
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IOptionsMonitor<MongoDbSettings> options)
        {
            var mongoClient = new MongoClient(options.CurrentValue.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(options.CurrentValue.DatabaseName);

            _users = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task CreateAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}
