namespace Binel.Models
{
    public class PostCreateViewModel
    {
        public int? OrganizationId { get; set; }
        public string? Title { get; set; }
        public string? PostText { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? ExternalPlatform { get; set; }
        public List<IFormFile> UploadedFiles { get; set; }
    }
}
