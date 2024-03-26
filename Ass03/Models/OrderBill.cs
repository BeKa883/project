using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ass03.Models
{
    public class OrderBill
    {
   
        public int OrderBillId { get; set; }

        public int UserId { get; set; } 

        public DateTime OrderDate { get; set; }
      
        public string ShippingAddress { get; set; }
      
        public string PhoneNumber { get; set; }

        public int TotalAmount { get; set; }

        public string description { get; set; }

       
    }
}
