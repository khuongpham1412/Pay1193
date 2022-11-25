using Microsoft.AspNetCore.Mvc;
using Pay1193.Services;
using Pay1193.Entity;
using Pay1193.Models;

namespace Pay1193.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IEmployee employeeService, IWebHostEnvironment webHostEnvironment)
        {
            _employeeService = employeeService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var employee = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
                Gender = employee.Gender,
                ImageUrl = employee.ImageUrl,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
                Email = employee.Email
            }).ToList();
            return View(employee);
        }

        public IActionResult Detail(int id)
        {
            var employee = _employeeService.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }
            EmployeeDetailsViewModel model = new EmployeeDetailsViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
                Gender = employee.Gender,
                ImageUrl = employee.ImageUrl,
                DoB = employee.DoB,
                Designation = employee.Designation,
                Email = employee.Email,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                UnionMember = employee.UnionMember,
                Address = employee.Address,
                City = employee.City,
                PostCode = employee.PostCode
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new EmployeeCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    MiddleName = model.MidleName,
                    LatName = model.LastName,
                    Gender = model.Gender,
                    DoB = model.DOB,
                    DateJoined = model.DateJoined,
                    Designation = model.Designation,
                    NationalInsuranceNo = model.NationalInsuranceNo,
                    StudentLoan = model.StudentLoan,
                    PaymentMethod = model.PaymentMethod,
                    UnionMember= model.UnionMember,
                    Phone = model.Phone,
                    Address = model.Address,
                    City = model.City,
                    PostCode = model.Postcode,
                    Email = model.Email,
                    EmployeeNo = model.EmployeeNo,
                    FullName = model.FullName
                };
                if(model.ImageUrl != null & model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employees";
                    var filename = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webrootPath = _webHostEnvironment.WebRootPath;
                    filename = DateTime.UtcNow.ToString("yymmssfff") + filename + extension;
                    var path = Path.Combine(webrootPath,uploadDir, filename);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + filename;

                }
                await _employeeService.CreateAsync(employee);
                return RedirectToAction("Index");
            }
            return View(); 
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LatName,
                MidleName = employee.MiddleName,
                EmployeeNo = employee.EmployeeNo,
                Gender = employee.Gender,
                Email = employee.Email,
                DOB = employee.DoB,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                UnionMember = employee.UnionMember,
                Address = employee.Address,
                City = employee.City,
                Postcode = employee.PostCode
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    MiddleName = model.MidleName,
                    LatName = model.LastName,
                    Gender = model.Gender,
                    DoB = model.DOB,
                    DateJoined = model.DateJoined,
                    Designation = model.Designation,
                    NationalInsuranceNo = model.NationalInsuranceNo,
                    StudentLoan = model.StudentLoan,
                    PaymentMethod = model.PaymentMethod,
                    UnionMember = model.UnionMember,
                    Phone = model.Phone,
                    Address = model.Address,
                    City = model.City,
                    PostCode = model.Postcode,
                    Email = model.Email,
                    EmployeeNo = model.EmployeeNo,
                    FullName = model.FullName
                };
                if (model.ImageUrl != null & model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employees";
                    var filename = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webrootPath = _webHostEnvironment.WebRootPath;
                    filename = DateTime.UtcNow.ToString("yymmssfff") + filename + extension;
                    var path = Path.Combine(webrootPath, uploadDir, filename);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + filename;

                }
                await _employeeService.UpdateAsync(employee);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _employeeService.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeDeleteViewModel
            {
                Id = employee.Id,
                FullName = employee.FullName
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeDeleteViewModel model)
        {
            await _employeeService.Delete(model.Id);
            return RedirectToAction("Index");
        }
        #region API

        public IActionResult Getall()
        {
            var employeeList = _employeeService.GetAll();
            return Json(new {data = employeeList});
        }

        #endregion
    }
}
