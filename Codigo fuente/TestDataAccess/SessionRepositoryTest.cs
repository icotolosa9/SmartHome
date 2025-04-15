using DataAccess.Context;
using DataAccess.Repositories;
using Domain;
using IDataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataAccess
{
    [TestClass]
    public class SessionRepositoryTest
    {
        private SqliteConnection _connection;
        private SmartHomeContext _context;
        private SessionRepository _sessionRepository;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var contextOptions = new DbContextOptionsBuilder<SmartHomeContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new SmartHomeContext(contextOptions);
            _context.Database.EnsureCreated();

            _sessionRepository = new SessionRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _connection.Close();
        }

        [TestMethod]
        public void SaveSessionMockTest()
        {
            // Arrange
            Session session = new Session { Token = Guid.NewGuid(), UserEmail = "user@example.com" };

            var mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<Session>>();

            mockContext.Setup(ctx => ctx.Sessions).Returns(mockDbSet.Object);
            ISessionRepository sessionRepository = new SessionRepository(mockContext.Object);

            // Act
            sessionRepository.Save(session);

            // Assert
            mockDbSet.Verify(dbSet => dbSet.Add(It.IsAny<Session>()), Times.Once);  
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once); 
        }

        [TestMethod]
        public void GetByTokenMockTest()
        {
            // Arrange
            Guid token = Guid.NewGuid();
            Session expectedSession = new Session { Token = token, UserEmail = "user@example.com" };

            var mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<Session>>();


            var sessions = new List<Session> { expectedSession }.AsQueryable();

            mockDbSet.As<IQueryable<Session>>().Setup(m => m.Provider).Returns(sessions.Provider);
            mockDbSet.As<IQueryable<Session>>().Setup(m => m.Expression).Returns(sessions.Expression);
            mockDbSet.As<IQueryable<Session>>().Setup(m => m.ElementType).Returns(sessions.ElementType);
            mockDbSet.As<IQueryable<Session>>().Setup(m => m.GetEnumerator()).Returns(sessions.GetEnumerator());

            mockContext.Setup(ctx => ctx.Sessions).Returns(mockDbSet.Object);
            ISessionRepository sessionRepository = new SessionRepository(mockContext.Object);

            // Act
            Session result = sessionRepository.GetByToken(token);

            // Assert
            Assert.AreEqual(expectedSession, result);  
        }

        [TestMethod]
        public void GetByToken_NoSessionFound_ReturnsNull()
        {
            // Arrange
            Guid token = Guid.NewGuid();

            var mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<Session>>();

            var sessions = new List<Session>().AsQueryable();

            mockDbSet.As<IQueryable<Session>>().Setup(m => m.Provider).Returns(sessions.Provider);
            mockDbSet.As<IQueryable<Session>>().Setup(m => m.Expression).Returns(sessions.Expression);
            mockDbSet.As<IQueryable<Session>>().Setup(m => m.ElementType).Returns(sessions.ElementType);
            mockDbSet.As<IQueryable<Session>>().Setup(m => m.GetEnumerator()).Returns(sessions.GetEnumerator());

            mockContext.Setup(ctx => ctx.Sessions).Returns(mockDbSet.Object);
            ISessionRepository sessionRepository = new SessionRepository(mockContext.Object);

            // Act
            Session result = sessionRepository.GetByToken(token);

            // Assert
            Assert.IsNull(result);  
        }
    }
}
