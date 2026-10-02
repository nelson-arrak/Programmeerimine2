using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Id), IsUnique = true)]
    public class Order
    {
        [Required]
        public int Id {get;set;}
        
        [Required]
        public Customer Customer {get;set;}
        
        [Required]
        public DateTime OrderDate {get;set;}
        
        [Required]
        [StringLength(20)]
        public string Status {get;set;}
        
        [Required]
        public List<OrderItem> OrderItems {get;set;}
    }
}
