using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services.Implement
{
    public class NationalInsureanceService : INationalInsuranceService
    {
        private decimal NIRate;
        private decimal NIC;
        public decimal NIContribution(decimal totalAmount)
        {
            if(totalAmount < 758)
            {
                NIRate = 0m;
                NIC = 0m;
            }
            else if(totalAmount >=758 && totalAmount <= 4189)
            {
                NIRate = .13m;
                NIC = (totalAmount - 758) * NIRate;
            }
            else if(totalAmount > 4189)
            {

                NIRate = .13m;
                NIC = ((4189 - 758) * .13m) + (totalAmount - 4189) * NIRate;
            }
            return NIC;
        }
    }
}
