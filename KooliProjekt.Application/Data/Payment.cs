using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Order), IsUnique = true)]
    public class Payment
    {
        public int Id {get;set;}
        
        [Required]
        public Order Order {get;set;}
        
        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal Amount {get;set;}
        
        public DateTime PaymentDate {get;set;}
        
        [Required]
        [StringLength(50)]
        public string PaymentMethod {get;set;}
        
        [Required]
        public bool Status {get;set;}
    }
}
