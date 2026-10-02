using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KooliProjekt.Application.Data
{
    public class Address
    {
        public int Id {get;set;}

        [Required]
        [StringLength(100)]
        public string Street {get;set;}

        [Required]
        [StringLength(100)]
        public string City {get;set;}
        
        [Required]
        [StringLength(100)]
        public string State {get;set;}
        
        [Required]
        [StringLength(20)]
        public string ZipCode {get;set;}
        
        [Required]
        [StringLength(100)]
        public string Country {get;set;}
    }
}
