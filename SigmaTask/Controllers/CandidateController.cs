using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace SigmaTask.Controllers
{
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateRepository _repository;
        private readonly IMemoryCache _cache;


        public CandidatesController(ICandidateRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }
        /// <summary>
        /// Add and Update Candidate
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddOrUpdateCandidateAsync(candidate);
            return Ok("Candidate processed successfully.");
        }
        /// <summary>
        /// To get a cache data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public IActionResult GetCacheItem(string key)
        {
            if (_cache.TryGetValue(key, out var cacheItem))
            {
                return Ok(cacheItem); 
            }
            else
            {
                return NotFound("Cache item not found");
            }
        }
    }


}