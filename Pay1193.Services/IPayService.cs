using Microsoft.AspNetCore.Mvc.Rendering;
using Pay1193.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services
{
    public interface IPayService
    {
        Task CreateAsync(PaymentRecord paymentRecord);
        PaymentRecord GetById(int id);
        TaxYear GetTaxYearById(int id);
        IEnumerable<SelectListItem> GetAllTaxYear();
        IEnumerable<PaymentRecord> GetAll();
        decimal OverTimeHours(decimal hoursWorked, decimal contractualHours);
        decimal ContractualEarning(decimal contractualHours, decimal hoursWorked, decimal hourlyRate);
        decimal OvertimeRate(decimal hourlyRate);
        //update 22/11
        decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours);
        decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFees);
        decimal NetPay(decimal totalEarnings, decimal totalDeduction);
        decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings);
    }
}
