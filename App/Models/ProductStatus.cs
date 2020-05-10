using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCApp.Models
{
    public class ProductStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusType { get; set; }
        public string Status { get; set; }
    }
}
