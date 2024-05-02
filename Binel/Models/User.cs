using System;
using System.Collections.Generic;

namespace Binel.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? OrganizationId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? Birth { get; set; }

    public string PasswordHash { get; set; }

    public virtual Organization? Organization { get; set; }

    public virtual ICollection<DonateLog> Logs { get; set; } = new List<DonateLog>();
}
