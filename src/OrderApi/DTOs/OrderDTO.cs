using System;
using System.Collections.Generic;
using OrderApi.Models;

namespace OrderApi.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public Address Address { get; set; }
        public string PaymentAuthCode { get; set; }
        public decimal Total { get; set; }
        public string Note { get; set; }
        public IEnumerable<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}