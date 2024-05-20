using System;
using System.Collections.Generic;

namespace Binel.Models;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    public string? OrganizationTitle { get; set; }

    public string? Biography { get; set; }

    public virtual ICollection<DonatePost> DonatePosts { get; set; } = new List<DonatePost>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
