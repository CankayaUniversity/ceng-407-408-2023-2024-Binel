using System;
using System.Collections.Generic;

namespace Binel.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? OrganizationId { get; set; }

    public string? Title { get; set; }

    public string? PostText { get; set; }

    public DateTime? PublishDate { get; set; }

    public string? ExternalPlatform { get; set; }

    public virtual Organization? Organization { get; set; }

    public virtual ICollection<FileUrl> Files { get; set; } = new List<FileUrl>();
}
