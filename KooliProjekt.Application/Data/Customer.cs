using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class Customer
    {
        [Required]
        public int Id {get;set;}
        
        [Required]
        public List<Address> Addresses {get;set;}
        
        [Required]
        [StringLength(100)]
        public string Name {get;set;}
        
        [Required]
        [StringLength(100)]
        public string Email {get;set;}
        
        [Required]
        public string PasswordHash {get;set;}
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber {get;set;}

        [Required]
        public List<Order> Orders {get;set;}
    }
}
