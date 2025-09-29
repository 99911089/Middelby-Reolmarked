using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    namespace Reolmarked.Model
    {
        // Repræsenterer et salg med kommission
        public class Sale
        {
            public double SalePrice { get; set; }           // Salgspris
            public double CommissionPercent { get; set; } = 10; // Kommission i procent

            // Beregn kommission
            public double CalculateCommission()
            {
                return SalePrice * (CommissionPercent / 100);
            }
        }
    }

}
