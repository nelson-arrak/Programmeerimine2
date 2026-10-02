using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        [Required]
        public int Id {get;set;}
        
        [Required]
        [StringLength(20)]
        public string Name {get;set;}
    }
}
