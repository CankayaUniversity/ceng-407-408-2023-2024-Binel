using System;
using System.Collections.Generic;

namespace Binel.Models;

public partial class FileUrl
{
    public int FileId { get; set; }

    public string? FileUrl1 { get; set; }

    public virtual ICollection<DonatePost> Donates { get; set; } = new List<DonatePost>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
