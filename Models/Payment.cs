using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management_App.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentQuantity { get; set; }

        // Campo adicional opcional para mostrar el nombre del método de pago
        public string? PaymentMethodName { get; set; }
    }
}
