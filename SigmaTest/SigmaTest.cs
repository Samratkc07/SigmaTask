using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using SigmaTask;

namespace SigmaTest
{
    public class CandidateRepositoryTests
    {
        private CandidateDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CandidateDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new CandidateDbContext(options);
        }

        private Mock<IMemoryCache> CreateMockCache()
        {
            var mockCache = new Mock<IMemoryCache>();

            var cacheEntry = Mock.Of<ICacheEntry>();
            mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntry);
            mockCache.Setup(m => m.Remove(It.IsAny<object>()));
            return mockCache;
        }

        [Fact]
        public async Task AddOrUpdateCandidateAsync_ShouldAddNewCandidate_WhenCandidateDoesNotExist()
        {
            var dbContext = CreateDbContext();
            var mockCache = CreateMockCache();
            var repository = new CandidateRepository(dbContext, mockCache.Object);

            var candidate = new Candidate
            {
                FirstName = "Samrat",
                LastName = "kc",
                PhoneNumber = "9876543210",
                Email = "Samrat.kc@gmail.com",
                BestCallTime = "Afternoon",
                LinkedInUrl = "https://linkedin.com/in/johndoe",
                GitHubUrl = "https://github.com/johndoe",
                FreeTextComment = "Looking forward to the interview."
            };

            await repository.AddOrUpdateCandidateAsync(candidate);

            var dbCandidate = await dbContext.Candidates.SingleOrDefaultAsync(c => c.Email == candidate.Email);
            Assert.NotNull(dbCandidate);
            Assert.Equal(candidate.FirstName, dbCandidate.FirstName);

            mockCache.Verify(m => m.CreateEntry(candidate.Email), Times.Once); 
        }

        [Fact]
        public async Task AddOrUpdateCandidateAsync_ShouldUpdateExistingCandidate_WhenCandidateAlreadyExists()
        {
            var dbContext = CreateDbContext();
            var mockCache = CreateMockCache();
            var repository = new CandidateRepository(dbContext, mockCache.Object);

            var existingCandidate = new Candidate
            {
                FirstName = "Samrat",
                LastName = "kc",
                PhoneNumber = "9876543210",
                Email = "Samrat.kc@gmail.com",
                BestCallTime = "Morning",
                LinkedInUrl = "https://linkedin.com/in/johndoe",
                GitHubUrl = "https://github.com/johndoe",
                FreeTextComment = "Initial comment."
            };
            dbContext.Candidates.Add(existingCandidate);
            await dbContext.SaveChangesAsync();

            var updatedCandidate = new Candidate
            {
                FirstName = "Samrat",
                LastName = "kc",
                PhoneNumber = "98765432152",
                Email = "Samrat.kc@gmail.com", 
                BestCallTime = "Afternoon",
                LinkedInUrl = "https://linkedin.com/in/ss",
                GitHubUrl = "https://github.com/ss",
                FreeTextComment = "Updated comment."
            };

            await repository.AddOrUpdateCandidateAsync(updatedCandidate);

            var dbCandidate = await dbContext.Candidates.SingleOrDefaultAsync(c => c.Email == updatedCandidate.Email);
            Assert.NotNull(dbCandidate);
            Assert.Equal("Samrat", dbCandidate.FirstName);
            Assert.Equal("98765432152", dbCandidate.PhoneNumber);
            Assert.Equal("Afternoon", dbCandidate.BestCallTime);

            mockCache.Verify(m => m.Remove(updatedCandidate.Email), Times.Once); 
            mockCache.Verify(m => m.CreateEntry(updatedCandidate.Email), Times.Once); 
        }

        [Fact]
        public async Task AddOrUpdateCandidateAsync_ShouldSetCacheForNewCandidate()
        {
            var dbContext = CreateDbContext();
            var mockCache = CreateMockCache();
            var repository = new CandidateRepository(dbContext, mockCache.Object);

            var candidate = new Candidate
            {
                FirstName = "Samrat",
                LastName = "kc",
                PhoneNumber = "9876543210",
                Email = "Samrat.kc@gmail.com",
                BestCallTime = "Evening",
                LinkedInUrl = "https://linkedin.com/in/johnsmith",
                GitHubUrl = "https://github.com/johnsmith",
                FreeTextComment = "This is a new candidate."
            };

            await repository.AddOrUpdateCandidateAsync(candidate);
            mockCache.Verify(m => m.CreateEntry(candidate.Email), Times.Once);
        }
    }
}