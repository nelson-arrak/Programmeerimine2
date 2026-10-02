using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(Order), IsUnique = true)]
    public class Payment
    {
        [Required]
        public int Id {get;set;}
        
        [Required]
        public Order Order {get;set;}
        
        [Required]
        public decimal Amount {get;set;}
        
        [Required]
        public DateTime PaymentDate {get;set;}
        
        [Required]
        [StringLength(50)]
        public string PaymentMethod {get;set;}
        
        [Required]
        public bool Status {get;set;}
    }
}
