using System;
using System.Collections.Generic;

namespace Binel.Models;

public partial class Category
{
    
    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<DonatePost> Donates { get; set; } = new List<DonatePost>();
}
