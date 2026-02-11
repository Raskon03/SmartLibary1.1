using Moq;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Application.Services;
using SmartLibrary.Domain;
using Xunit;

namespace SmartLibrary.Tests
{
    public class LendingServiceTests
    {
        // 1. Дефинираме Mock обектите (фалшивите репозиторита)
        private readonly Mock<IBookRepository> _bookRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly LendingService _service;

        public LendingServiceTests()
        {
            // Инициализираме ги преди всеки тест
            _bookRepoMock = new Mock<IBookRepository>();
            _userRepoMock = new Mock<IUserRepository>();

            // Подаваме фалшивите репозиторита на истинския сървис
            _service = new LendingService(_bookRepoMock.Object, _userRepoMock.Object);
        }

        // --- СЦЕНАРИЙ 1: ПОЗИТИВЕН ---
        // Успешно наемане, когато книгата е свободни и потребителят съществува
        [Fact]
        public async Task RentBook_ShouldReturnTrue_WhenUserExistsAndBookIsAvailable()
        {
            // Arrange (Подготовка)
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            // Казваме на Mock-а: "Ако някой потърси този userId, върни валиден User"
            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(new User { Id = userId, FullName = "Test User" });

            // Казваме на Mock-а: "Ако някой потърси този bookId, върни СВОБОДНА книга"
            _bookRepoMock.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(new Book { Id = bookId, IsAvailable = true, Title = "Test Book" });

            // Act (Действие)
            var result = await _service.RentBookAsync(userId, bookId);

            // Assert (Проверка)
            Assert.True(result); // Очакваме true

            // Проверяваме дали методът UpdateAsync е бил извикан веднъж (т.е. дали сме записали промяната)
            _bookRepoMock.Verify(repo => repo.UpdateAsync(It.Is<Book>(b => b.IsAvailable == false)), Times.Once);
        }

        // --- СЦЕНАРИЙ 2: НЕГАТИВЕН ---
        // Неуспешно наемане, когато книгата вече е заета
        [Fact]
        public async Task RentBook_ShouldReturnFalse_WhenBookIsNotAvailable()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(new User { Id = userId });

            // Тук връщаме книга, която е ЗАЕТА (IsAvailable = false)
            _bookRepoMock.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(new Book { Id = bookId, IsAvailable = false });

            // Act
            var result = await _service.RentBookAsync(userId, bookId);

            // Assert
            Assert.False(result); // Очакваме false

            // Уверяваме се, че НИКОГА не сме извикали UpdateAsync (не трябва да пипаме базата)
            _bookRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }
    }
}