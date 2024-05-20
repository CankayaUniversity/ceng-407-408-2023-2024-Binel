using System;
using System.Collections.Generic;

namespace Binel.Models;

public partial class DonatePost
{
    public int DonateId { get; set; }

    public int OrganizationId { get; set; }

    public string? Title { get; set; }

    public string? DonateText { get; set; }

    public DateTime? PublishDate { get; set; }

    public int? TotalDonate { get; set; }

    public int? MaxLimit { get; set; }

    public int? MinLimit { get; set; }

    public virtual ICollection<DonateLog> DonateLogs { get; set; } = new List<DonateLog>();

    public virtual Organization Organization { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<FileUrl> Files { get; set; } = new List<FileUrl>();
}
