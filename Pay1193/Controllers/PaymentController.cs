using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Services;
using Pay1193.Services.Implement;
using RotativaCore;

namespace Pay1193.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IEmployee _employee;
        private readonly IPayService _payService;
        private readonly INationalInsuranceService _nationalInsuranceService;
        private readonly ITaxService _taxService;
        private decimal overtimeHrs;
        private decimal contractualEarnings;
        private decimal overtimeEarnings;
        private decimal nationalInsurance;
        private decimal totalEarnings;
        private decimal tax;
        private decimal unionFee;
        private decimal studentLoan;
        private decimal totalDeduction;
        public PaymentController(IEmployee employee, IPayService payService, INationalInsuranceService nationalInsuranceService, ITaxService taxService)
        {
            _employee = employee;
            _payService = payService;
            _nationalInsuranceService = nationalInsuranceService;
            _taxService = taxService;
        }
        public IActionResult Index()
        {
            var payRecords = _payService.GetAll().Select(payment => new PaymentRecordIndexViewModel
            {
                Id = payment.Id,
                EmployeeId = payment.EmployeeId,
                FullName = _employee.GetById(payment.Id).FullName,
                PayDate = payment.DatePay,
                PayMonth = payment.MonthPay,
                TaxYearId = payment.TaxYearId,
                Year = _payService.GetTaxYearById(payment.TaxYearId).YearOfTax,
                TotalEarnings = payment.TotalEarnings,
                TotalDeduction = payment.EarningDeduction,
                NetPayment = payment.NetPayment,
                Employee = payment.Employee
            });
            return View(payRecords);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> employees = _employee.GetAllEmployeesForPayroll();
            IEnumerable<SelectListItem> taxYears = _payService.GetAllTaxYear();
            ViewBag.Employees = employees;
            ViewBag.TaxYears = taxYears;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            var payrecord = new PaymentRecord()
            {
                Id = model.Id,
                EmployeeId = model.EmployeeId,
                DatePay = model.PayDate,
                MonthPay = model.PayMonth,
                TaxYearId = model.TaxYearId,
                TaxCode = model.TaxCode,
                HourlyRate = model.HourlyRate,
                HourWorked = model.HourlyWorker,
                ContractualHours = model.ContractualHours,
                OvertimeHours = overtimeHrs = _payService.OverTimeHours(model.HourlyWorker, model.ContractualHours),
                ContractualEarnings = contractualEarnings = _payService.ContractualEarning(model.ContractualHours, model.HourlyWorker, model.HourlyRate),
                OvertimeEarnings = overtimeEarnings = _payService.OvertimeEarnings(_payService.OvertimeRate(model.HourlyRate), overtimeHrs),
                TotalEarnings = totalEarnings = _payService.TotalEarnings(overtimeEarnings, contractualEarnings),
                NiC = nationalInsurance = _nationalInsuranceService.NIContribution(totalEarnings),
                Tax = tax = _taxService.TaxAmount(totalEarnings),
                EarningDeduction = totalDeduction = _payService.TotalDeduction(tax, nationalInsurance, studentLoan, unionFee),
                UnionFee = unionFee = _employee.UnionFee(model.EmployeeId),
                SLC = studentLoan = _employee.StudentLoanRepaymentAmount(model.EmployeeId, totalEarnings),
                NetPayment = _payService.NetPay(totalEarnings, totalDeduction)
            };
            await _payService.CreateAsync(payrecord);
            return RedirectToAction("Index");
            //}
            ViewBag.employees = _employee.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payService.GetAllTaxYear();
            return View(model);
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var payment = _payService.GetById(id);
            if (payment != null)
            {
                var model = new PaymentRecordDetailViewModel()
                {
                    Id = payment.Id,
                    EmployeeId = payment.EmployeeId,
                    FullName = _employee.GetById(payment.EmployeeId).FullName,
                    Nino = _employee.GetById(payment.EmployeeId).NationalInsuranceNo,
                    Address = _employee.GetById(payment.EmployeeId).Address,
                    City = _employee.GetById(payment.EmployeeId).City,
                    PostCode = _employee.GetById(payment.EmployeeId).PostCode,
                    PayDate = payment.DatePay,
                    PayMonth = payment.MonthPay,
                    TaxYearId = payment.TaxYearId,
                    Year = _payService.GetTaxYearById(payment.TaxYearId).YearOfTax,
                    TaxCode = payment.TaxCode,
                    HourlyRate = payment.HourlyRate,
                    HoursWorker = payment.HourWorked,
                    ContractualHours = payment.ContractualHours,
                    OvertimeHours = payment.OvertimeHours,
                    OvertimeRate = _payService.OvertimeRate(payment.HourlyRate),
                    ContractualEarnings = payment.ContractualEarnings,
                    OvertimeEarnings = payment.OvertimeEarnings,
                    Tax = payment.Tax,
                    NIC = payment.NiC,
                    UnionFee = payment.UnionFee,
                    SLC = payment.SLC,
                    TotalEarnings = payment.TotalEarnings,
                    TotalDeduction = payment.EarningDeduction,
                    Employee = payment.Employee,
                    TaxYear = payment.TaxYear,
                    NetPayment = payment.NetPayment
                };
                return View(model);
            }
            return NotFound();
        }


        public IActionResult ExportPDF(int id)
        {
            var payslip = new ActionAsPdf("Detail", new { id = id })
            {
                FileName = "Payslip.pdf"
            };
            return payslip;
        }

    }
}
