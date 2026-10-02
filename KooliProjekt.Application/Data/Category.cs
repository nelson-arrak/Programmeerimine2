using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int Id {get;set;}
        
        [Required]
        [StringLength(50)]
        public string Name {get;set;}
    }
}
