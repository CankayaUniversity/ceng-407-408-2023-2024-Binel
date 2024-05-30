using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Binel.Models
{
    public class DonateCategory
    {
        public int DonateId { get; set; }
        
        public int CategoryId { get; set; }
        
        [ForeignKey("DonateId")]
        public virtual DonatePost DonatePost { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
