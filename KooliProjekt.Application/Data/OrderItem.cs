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
        
        public int Quantity {get;set;}
        public decimal Price {get;set;}
    }
}
