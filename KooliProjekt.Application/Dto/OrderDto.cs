using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public IList<OrderLineDto> Items { get; set; }

        public OrderDto()
        {
            Items = new List<OrderLineDto>();
        }
    }
}
