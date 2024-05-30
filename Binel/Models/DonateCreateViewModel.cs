

namespace Binel.Models;
public class DonateCreateViewModel
{
    public int OrganizationId { get; set; }
    public string? Title { get; set; }
    public string? DonateText { get; set; }
    public DateTime? PublishDate { get; set; }
    public int? TotalDonate { get; set; }
    public int? MaxLimit { get; set; }
    public int? MinLimit { get; set; }
    public string[] Categories { get; set; } 
    public List<IFormFile> UploadedFilesDonate { get; set; }

}
