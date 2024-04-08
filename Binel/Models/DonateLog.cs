using System;
using System.Collections.Generic;

namespace Binel.Models;

public partial class DonateLog
{
    public int LogId { get; set; }

    public int DonateId { get; set; }

    public DateTime? DonateDate { get; set; }

    public int? Amount { get; set; }

    public virtual DonatePost Donate { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
