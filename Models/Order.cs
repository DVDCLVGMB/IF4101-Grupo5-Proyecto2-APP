using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management_App.Models
{
    public class Order
    {
        public int ClientId { get; set; }
        public int EmployeeId { get; set; } // Este lo pasas como 4 (quemado)
        public int CityId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CreditCardNumber { get; set; } = string.Empty;

    }
}
