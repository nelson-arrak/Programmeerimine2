using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class OrderItem
    {
        public int Id {get;set;}
        
        [Required]
        public Product Product {get;set;}
        
        [Range(0, int.MaxValue)]
        public int Quantity {get;set;}

        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal Price {get;set;}
    }
}
