using System.ComponentModel.DataAnnotations;

namespace SigmaTask
{
    public class Candidate
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? BestCallTime { get; set; }

        [Url]
        public string? LinkedInUrl { get; set; }

        [Url]
        public string? GitHubUrl { get; set; }

        [Required]
        [MaxLength(500)]
        public string? FreeTextComment { get; set; }
    }

}