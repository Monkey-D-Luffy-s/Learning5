using Learning5.Models.Account;
using Learning5.Models.PaySlabs;
using Learning5.services.Account;
using Learning5.services.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Learning5.Controllers
{
    public class PayslabsController : Controller
    {
        private readonly IAcount _account;
        private readonly IPaymentService _paymentService;
        public PayslabsController(IAcount account, IPaymentService paymentService)
        {
            _account = account;
            _paymentService = paymentService;
        }


        public async Task<IActionResult> AddBankDetails()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Employees = await _account.GetEmployeesForAdminDropdown();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeBankDetails(BankDetails bankDetails)
        {
            if (ModelState.IsValid)
            {
                var result = await _paymentService.AddBankDetails(bankDetails);
                if (result.Contains("Successfully"))
                {
                    TempData["SuccessMessage"] = "Bank details added successfully.";

                }
                else
                {
                    TempData["ErrorMessage"] = "Bank details added successfully.";

                }
               
            }
            return RedirectToAction("AddBankDetails");
        }

        [HttpGet]
        public async Task<JsonResult> GetBankDetails(string employeeId)
        {
            return await _paymentService.GetBankDetails(employeeId);
        }
        public async Task<IActionResult> AddEmployeeSalaryDetails()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Employees = await _account.GetEmployeesForAdminDropdown();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeSalaryDetails(EmployeeSalary bankDetails)
        {
            if (ModelState.IsValid)
            {
                var result = await _paymentService.AddEmployeeSalaryDetails(bankDetails);
                if (result.Contains("Successfully"))
                {
                    TempData["SuccessMessage"] = "Salary details added successfully.";

                }
                else
                {
                    TempData["ErrorMessage"] = "Salary details added successfully.";

                }
               
            }
            return RedirectToAction("AddEmployeeSalaryDetails");
        }

        [HttpGet]
        public async Task<JsonResult> GetEmpSalaryDetails(string employeeId)
        {
            return await _paymentService.GetEmployeeSalaryDetails(employeeId);
        }
        public async Task<IActionResult> AddEmployeeTaxDeductionDetails()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Employees = await _account.GetEmployeesForAdminDropdown();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeTaxDeductionDetails(EmployeeTaxDeclarations bankDetails)
        {
            if (ModelState.IsValid)
            {
                if (bankDetails.InvestmentProofFile != null )
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Taxdelcaration");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + bankDetails.InvestmentProofFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await bankDetails.InvestmentProofFile.CopyToAsync(stream);
                    }
                    bankDetails.InvestmentProof = "/Taxdelcaration/" + uniqueFileName;
                    try
                    {
                        var result = await _paymentService.AddEmployeeTaxDeclarationDetails(bankDetails);
                        if (result.Contains("Successfully"))
                        {
                            TempData["SuccessMessage"] = "TaxDeclaration details added successfully.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "TaxDeclaration details added successfully.";
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                    }
                }
               
            }
            return RedirectToAction("AddEmployeeTaxDeductionDetails");
        }

        [HttpGet]
        public async Task<JsonResult> GetEmpTaxDeductionDetails(string employeeId)
        {
            return await _paymentService.GetEmployeeTaxDeclarationDetails(employeeId);
        }
        public async Task<IActionResult> AddEmployeeStaturaryDetails()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Employees = await _account.GetEmployeesForAdminDropdown();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeStaturaryDetails(StaturaryDetailsEmployee bankDetails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _paymentService.AddEmployeeStaturaryDetails(bankDetails);
                    if (result.Contains("Successfully"))
                    {
                        TempData["SuccessMessage"] = "Staturay details added successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Staturay details added successfully.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return RedirectToAction("AddEmployeeStaturaryDetails");
        }

        [HttpGet]
        public async Task<JsonResult> GetEmpStaturaryDetails(string employeeId)
        {
            return await _paymentService.GetEmployeeStaturaryDetails(employeeId);
        }
    }
}
