using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Description), IsUnique = true)]
    public class Product
    {
        [Required]
        public int Id {get;set;}
        
        [Required]
        [StringLength(100)]
        public string Name {get;set;}
        
        [Required]
        [StringLength(255)]
        public string Description {get;set;}
        
        [Required]
        public decimal Price {get;set;}
        
        [Required]
        public int StockQuantity {get;set;}
        
        [Required]
        public Category Category {get;set;}
    }
}
