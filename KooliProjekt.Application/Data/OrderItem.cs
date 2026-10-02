using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(Product), IsUnique = true)]
    public class OrderItem
    {
        [Required]
        public int Id {get;set;}
        
        [Required]
        public Product Product {get;set;}
        
        [Required]
        public int Quantity {get;set;}
        
        [Required]
        public decimal Price {get;set;}
    }
}
