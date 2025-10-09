using Learning5.data;
using Learning5.Models.Account;
using Learning5.services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learning5.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAcount _account;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAcount _context, ILogger<AccountController> logger)
        {
            _account = _context;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser()
        {
            string username = User.Identity.Name;
            _logger.LogInformation($"Accessed AddUser page to {username} at {DateTime.Now}");
            ViewBag.Colleges = await _account.GetCollegesForDropdown();
            ViewBag.Designations = await _account.GetDesignationsForDropdown();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Passport_Document != null && user.Singnature_Document != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + user.Passport_Document.FileName;
                    var uniquesignFileName = Guid.NewGuid().ToString() + "_" + user.Singnature_Document.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    var singPath = Path.Combine(uploadsFolder, uniquesignFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.Passport_Document.CopyToAsync(stream);
                    }
                    using (var stream = new FileStream(singPath, FileMode.Create))
                    {
                        await user.Singnature_Document.CopyToAsync(stream);
                    }

                    user.Passport_DocumentPath = "/uploads/" + uniqueFileName;
                    user.Singnature_DocumentPath = "/uploads/" + uniquesignFileName;
                    try
                    {
                        var result = await _account.RegisterUser(user);
                        if (result.ToString().Contains("successfully"))
                        {
                            TempData["SuccessMessage"] = result.ToString();


                        }
                        else
                        {
                            TempData["ErrorMessage"] = result.ToString();

                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;


                    }

                }

            }
            else
            {

                TempData["ErrorMessage"] = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                ViewBag.Message = "There are validation errors.";
            }
            ViewBag.Colleges = await _account.GetCollegesForDropdown();
            ViewBag.Designations = await _account.GetDesignationsForDropdown();
            return View(new User());

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUser(string userId)
        {
            if (userId != null)
            {
                var result = await _account.DeleteUser(userId);
                if (result.ToString().Contains("Successfully"))
                {
                    TempData["SuccessMessage"] = result.ToString();
                    return RedirectToAction("AddUser");
                }
                else
                {
                    TempData["ErrorMessage"] = result.ToString();
                    return RedirectToAction("AddUser");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "User not Found";
            }
            return RedirectToAction("AddUser");
        }
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            var result = await _account.Logout();
            TempData["SuccessMessage"] = result.ToString();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser user)
        {
            var result = await _account.Login(user);
            if (result.ToString().Contains("successfully."))
            {
                TempData["SuccessMessage"] = result.ToString();
                return View();
            }
            else
            {
                TempData["ErrorMessage"] = result.ToString();
                return View();
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRole(Roles role)
        {
            try
            {
                var result = await _account.AddRole(role);
                TempData["SuccessMessage"] = result.ToString();
                return RedirectToAction("AddRole");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message.ToString();
                return RedirectToAction("AddRole");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetRoles()
        {
            return Json(await _account.GetRoles());
        }
        [Authorize]
        public async Task<JsonResult> GetEmployees()
        {
            List<User> users = new List<User>();
            try
            {
                users = await _account.GetEmployeesList();

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message.ToString();
                throw;
            }
            return Json(users);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole()
        {
            ViewBag.Roles = await _account.GetRolesForDropdown();
            ViewBag.Employees = await _account.GetEmployeesForDropdown();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _account.AssignRole(obj);
                    TempData["SuccessMessage"] = result.ToString();
                    return RedirectToAction("AssignRole");
                }
                else
                {
                    return View(obj);
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message.ToString();
                return View(obj);
            }
        }

        [Authorize]
        public IActionResult ApplyLeave()
        {
            string username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                // The user is not authenticated
                return RedirectToAction("Login");
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ApplyLeave(LeavesModule leave)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string username = User.Identity.Name;

                    if (string.IsNullOrEmpty(username))
                    {
                        // The user is not authenticated
                        return RedirectToAction("Login");
                    }
                    leave.UserName = username;
                    var result = await _account.ApplyLeave(leave);
                    if (result.ToString().Contains("Successfully"))
                    {
                        TempData["SuccessMessage"] = result.ToString();
                        return RedirectToAction("ApplyLeave");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = result.ToString();
                        return RedirectToAction("ApplyLeave");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return View();
                }
            }
            else
            {
                TempData["ErrorMessage"] = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                ViewBag.Message = "There are validation errors.";
                return RedirectToAction("ApplyLeave");
            }


        }
        [Authorize]
        public IActionResult LeavesHistory()
        {
            string username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                // The user is not authenticated
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            List<LeavesModule> leaves = new List<LeavesModule>();
            try
            {
                string username = User.Identity.Name;


                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login", "Account");
                }
                leaves = await _account.GetLeavesList(username);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message.ToString();
                throw;
            }
            return Json(leaves);
        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> CancleLeave(string LeaveId)
        {
            try
            {
                var result = await _account.CancelLeave(LeaveId);
                if (result.ToString().Contains("Successffully"))
                {
                    TempData["SuccessMessage"] = result.ToString();

                }
                else
                {
                    TempData["ErrorMessage"] = result.ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message.ToString();

            }
            return Json("");
        }

        [Authorize]
        public async Task<IActionResult> LeaveBalances()
        {
            string username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                // The user is not authenticated
                return RedirectToAction("Login");
            }
            else
            {
                LaeveBalances obj = await _account.GetLeaveBalance(username);
                return View(obj);

            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddColleges()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddColleges(Colleges obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string username = User.Identity.Name;

                    if (string.IsNullOrEmpty(username))
                    {
                        // The user is not authenticated
                        return RedirectToAction("Login");
                    }
                    obj.AddedBy = username ?? "God";
                    var result = await _account.AddCollege(obj);
                    if (result.ToString().Contains("Successfully"))
                    {
                        TempData["SuccessMessage"] = result.ToString();
                        return RedirectToAction("AddColleges");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = result.ToString();
                        return RedirectToAction("AddColleges");
                    }
                }
                catch (Exception ex)
                {

                    TempData["ErrorMessage"] = ex.Message.ToString();
                }
            }
            else
            {
                TempData["ErrorMessage"] = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                ViewBag.Message = "There are validation errors.";
            }
            return RedirectToAction("AddColleges");

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetCollegesList()
        {
            return Json(await _account.GetCollegesList());
        }
        [Authorize(Roles = "Principal,DIEO,RJD")]
        [HttpGet]
        public async Task<JsonResult> GetLeavesForApproval()
        {
            string username = User.Identity.Name;
            return Json(await _account.GetLeavesForApproval(username));
        }

        [Authorize(Roles = "Principal,DIEO,RJD")]
        public IActionResult ApproveLeaves()
        {

            string username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            _logger.LogInformation($"Accessed ApproveLeaves page to {username} at {DateTime.Now}");
            return View();
        }
        [Authorize(Roles = "Principal,DIEO,RJD")]
        [HttpPost]
        public async Task<JsonResult> UpdateLeave(string LeaveId, string remarks, string flag)
        {
            try
            {
                string username = User.Identity.Name;
                if (!string.IsNullOrEmpty(LeaveId) && !string.IsNullOrEmpty(username))
                {
                    var result = await _account.ApproveLeave(LeaveId, username, remarks, flag);
                    if (result.ToString().Contains("Successfully"))
                    {
                        _logger.LogInformation($"Leave {LeaveId} approved by {username} at {DateTime.Now}");
                        TempData["SuccessMessage"] = result.ToString();
                        return Json("1");

                    }
                    else
                    {
                        _logger.LogWarning($"Failed to approve leave {LeaveId} by {username} at {DateTime.Now}");
                        TempData["ErrorMessage"] = result.ToString();
                        return Json("2");
                    }
                }
                else
                {
                    return Json("3");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while approving leave by {User.Identity.Name} at {DateTime.Now}");
                TempData["ErrorMessage"] = ex.Message.ToString();
                return Json("4");
            }
        }

        // adding principal approvals need to add college code for every employee then create dropdown for it

        [Authorize(Roles = "Admin")]
        public IActionResult AddDesignation()
        {

            string username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            _logger.LogInformation($"Accessed AddDesignation page to {username} at {DateTime.Now}");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddDesignation(Designations obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string username = User.Identity.Name;

                    if (string.IsNullOrEmpty(username))
                    {
                        // The user is not authenticated
                        return RedirectToAction("Login");
                    }
                    var result = await _account.AddDesignation(obj);
                    if (result.ToString().Contains("Successfully"))
                    {
                        TempData["SuccessMessage"] = result.ToString();
                        return RedirectToAction("AddDesignation");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = result.ToString();
                        return RedirectToAction("AddDesignation");
                    }
                }
                catch (Exception ex)
                {

                    TempData["ErrorMessage"] = ex.Message.ToString();
                }
            }
            else
            {
                TempData["ErrorMessage"] = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                ViewBag.Message = "There are validation errors.";
            }
            return RedirectToAction("AddDesignation");

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetDesignationList()
        {
            return Json(await _account.GetDesignationsList());
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string? ReturnUrl = null)
        {
            string username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            _logger.LogWarning($"AccessDenied page accessed by {username} at {DateTime.Now}");
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            string username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            Profile obj = await _account.GetProfile(username);
            return View(obj);
        }

        [Authorize]
        public async Task<IActionResult> IdCard()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            IdCardModel obj = await _account.getIdDetails(username);
            return View(obj);
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetUserDetails(string userId)
        {

            return Json(await _account.GetUserDetails(userId));
        }


        [Authorize]
        public async Task<IActionResult> TimeSheetEntry()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTimeSheetEntry(TimeSheet timesheetData)
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }
            _logger.LogInformation($"Timesheet data submitted by {username} at {DateTime.Now}: {timesheetData}");
            timesheetData.UserId = username;
            TempData["SuccessMessage"] = "Timesheet submitted successfully!";
            return RedirectToAction("TimeSheetEntry");

        }
    }
}
