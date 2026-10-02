using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        public int Id {get;set;}
        
        [Required]
        [StringLength(100)]
        public string Name {get;set;}
        
        [Required]
        public string Description {get;set;}
        
        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal Price {get;set;}
        
        [Range(0, int.MaxValue)]
        public int StockQuantity {get;set;}
        
        [Required]
        public Category Category {get;set;}
    }
}
