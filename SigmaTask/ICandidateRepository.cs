namespace SigmaTask
{
    public interface ICandidateRepository
    {
        Task AddOrUpdateCandidateAsync(Candidate candidate);
    }
}
