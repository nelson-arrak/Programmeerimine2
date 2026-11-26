using System;

namespace KooliProjekt.Application.Dto
{
    public class CustomerOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
