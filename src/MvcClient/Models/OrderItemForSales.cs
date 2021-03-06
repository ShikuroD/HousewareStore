using System;
using System.ComponentModel.DataAnnotations;

namespace MvcClient.Models
{
    public class OrderItemForSales : OrderItem
    {
        //additional info for sales
        public string BuyerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{dd/MM/yyyy hh:mm:ss tt}")]
        public DateTime OrderDate { get; set; }
        public Address Address { get; set; }
        public string PaymentAuthCode { get; set; }
    }
}