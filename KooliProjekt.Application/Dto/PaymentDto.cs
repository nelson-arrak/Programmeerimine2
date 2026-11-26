using System;

namespace KooliProjekt.Application.Dto
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
    }
}
