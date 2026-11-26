using System;

namespace KooliProjekt.Application.Dto
{
    public class OrderListItemDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
