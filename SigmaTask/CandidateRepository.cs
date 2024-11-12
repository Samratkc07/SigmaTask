using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace SigmaTask
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly CandidateDbContext _context;
        private readonly IMemoryCache _cache;

        public CandidateRepository(CandidateDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }


        public async Task AddOrUpdateCandidateAsync(Candidate candidate)
        {
            var existingCandidate = await _context.Candidates.SingleOrDefaultAsync(c => c.Email == candidate.Email);

            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.BestCallTime = candidate.BestCallTime;
                existingCandidate.LinkedInUrl = candidate.LinkedInUrl;
                existingCandidate.GitHubUrl = candidate.GitHubUrl;
                existingCandidate.FreeTextComment = candidate.FreeTextComment;
                _context.Candidates.Update(existingCandidate);
                _cache.Remove(candidate.Email);
            }
            else
            {
                await _context.Candidates.AddAsync(candidate);
            }

            await _context.SaveChangesAsync();
            _cache.Set(candidate.Email, candidate, TimeSpan.FromMinutes(5));
        }
    }
}
