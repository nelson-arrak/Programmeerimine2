using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class Order
    {
        public int Id {get;set;}
        
        [Required]
        public Customer Customer {get;set;}

        public DateTime OrderDate {get;set;}
        
        [Required]
        [StringLength(20)]
        public string Status {get;set;}
        
        [MinLength(1)]
        public List<OrderItem> OrderItems {get;set;}
    }
}
