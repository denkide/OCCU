using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Quantity on hand")]
        public int QuantityOnHand { get; set; }

        [Display(Name = "Is on backorder?")]
        public bool BackorderFlag { get; set; }
    }
}
