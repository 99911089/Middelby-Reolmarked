using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    namespace Reolmarked.Model
    {
        public enum PaymentType
        {
            Kort,
            Kontant,
            MobilePay
        }

        public class Payment
        {
            public int PaymentId { get; set; }
            public int CustomerId { get; set; }
            public decimal Amount { get; set; }
            public PaymentType Type { get; set; }
            public DateTime Date { get; set; }
        }
    }

}
