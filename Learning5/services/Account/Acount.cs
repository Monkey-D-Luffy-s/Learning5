using Learning5.data;
using Learning5.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Runtime.Intrinsics.X86;

namespace Learning5.services.Account
{
    public class Acount : IAcount
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly AppDbContext _db;

        public Acount(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Roles> roleManager, AppDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;  // ✅ Assign properly
        }

        public async Task<string> RegisterUser(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    var obj = await _db.Users.Where(u => u.UserName == user.UserName).Include(u => u.Designations).FirstOrDefaultAsync();
                    if(obj != null)
                    {
                        obj.EmployeeName = user.EmployeeName;
                        obj.DepartmentId = user.DepartmentId;
                        obj.DesignationId = user.DesignationId;
                        obj.PhoneNumber = user.PhoneNumber;
                        obj.Email = user.Email;
                        obj.AadhaarId = user.AadhaarId;
                        obj.PanId = user.PanId;
                        obj.Address = user.Address;
                        obj.CollegeCode = user.CollegeCode;
                        obj.BloodGroup = user.BloodGroup;
                        obj.DateOfBirth = user.DateOfBirth;
                        obj.DateOfJoining = user.DateOfJoining;
                        obj.Passport_DocumentPath = user.Passport_DocumentPath;
                        obj.Singnature_DocumentPath = user.Singnature_DocumentPath;

                        var result = await _db.SaveChangesAsync();
                        if (result > 0)
                        {
                            return "User Updated successfully.";
                            
                        }
                        else
                        {
                            return "Failed to Update the User";
                        }
                    }
                    else
                    {
                        return "User Not Found";
                    }
                }
                else
                {
                    var count = _db.Users.Count();
                    user.UserName = count < 1 ? "E10001" : "E1" + (count + 1).ToString("D4");

                    var result = await _userManager.CreateAsync(user, user.UserName + "Hrms@123");

                    return result.Succeeded
                        ? "User created successfully."
                        : "Error creating user: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
                   
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> DeleteUser(string userid)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userid);
                if (user != null) 
                {
                    user.IsActive = false;
                    await _db.SaveChangesAsync();
                    return "User Removed Successfully.";
                }
                else
                {
                    return $"User Not Found with '{userid}'";
                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }
        }
        public async Task<List<User>> GetEmployeesList()
        {
            List<User> list = new List<User>();
            try
            {
                list = await _db.Users
                    .AsNoTracking()
                    .Where(u => u.IsActive == true)
                    .Select(u => new User
                    {
                        UserName = u.UserName,
                        EmployeeName = u.EmployeeName,
                        Email = u.Email,
                        DateOfJoining = u.DateOfJoining,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }
        public async Task<string> Login(LoginUser obj)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == obj.UserName);

                if (user == null && user.IsActive == false) return "User not Found";

                var result = await _signInManager.PasswordSignInAsync(user, obj.Password, false, false);

                return result.Succeeded ? "LoggedIn successfully." : "Invalid UserName Or Password";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> Logout()
        {
            await _signInManager.SignOutAsync();
            return "LoggedOut Successfully.";
        }
        public async Task<string> AddRole(Roles role)
        {
            try
            {
                var roleexist = await _roleManager.FindByNameAsync(role.RoleName);
                if (roleexist == null)
                {
                    var count = await _db.Roles.CountAsync();
                    role.Name = role.RoleName;
                    role.RoleId = count < 1 ? "R1" : "R" + (count + 1).ToString();
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return "Role Added successfully.";
                    }
                    else
                    {
                        return "Failed to add the Role";
                    }
                }
                else
                {
                    return "Role Already Exist";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();

            }
        }

        public async Task<string> AssignRole(AssignRoleModel obj)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(obj.UserName);

                if (user == null)
                {
                    return $"Error: User '{obj.UserName}' not found.";
                }

                var roleExists = await _roleManager.RoleExistsAsync(obj.RoleName);

                if (!roleExists)
                {
                    return "Role Deos Not Exist";
                }

                var userInRole = await _userManager.IsInRoleAsync(user, obj.RoleName);
                if (userInRole)
                {
                    return $"User '{obj.UserName}' is already in the role '{obj.RoleName}'.";
                }
                var result = await _userManager.AddToRoleAsync(user, obj.RoleName);

                if (result.Succeeded)
                {
                    return $"Role '{obj.RoleName}' successfully assigned to user '{obj.UserName}'.";
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return $"Error assigning role: {errors}";
                }
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }
        public async Task<List<Roles>> GetRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new Roles
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                })
                .ToListAsync();

            return roles;
        }
        public async Task<List<SelectListItem>> GetRolesForDropdown()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleSelectList = roles.Select(r => new SelectListItem
            {
                Text = r.RoleId + "-" + r.RoleName,
                Value = r.RoleName
            }).ToList();

            return roleSelectList;
        }
        public async Task<List<SelectListItem>> GetCollegesForDropdown()
        {
            var colleges = await _db.Colleges.ToListAsync();
            var collegesSelectList = colleges.Select(r => new SelectListItem
            {
                Text = r.CollegeCode.ToString() + " - " + r.CollegeName,
                Value = r.CollegeCode.ToString()
            }).ToList();

            return collegesSelectList;
        }
        public async Task<List<SelectListItem>> GetEmployeesForDropdown()
        {
            var roles = await _userManager.Users.Where(u=> u.IsActive == true).ToListAsync();
            var roleSelectList = roles.Select(r => new SelectListItem
            {
                Text = r.UserName + "-" + r.EmployeeName,
                Value = r.UserName
            }).ToList();

            return roleSelectList;
        }
        public async Task<List<SelectListItem>> GetDesignationsForDropdown()
        {
            var roles = await _db.Designations.ToListAsync();
            var roleSelectList = roles.Select(r => new SelectListItem
            {
                Text = r.DesignationId + "-" + r.DesignationName,
                Value = r.DesignationId
            }).ToList();

            return roleSelectList;
        }

        public async Task<string> ApplyLeave(LeavesModule leave)
        {
            try
            {
                var count = await _db.LeavesModules.CountAsync();
                if (!string.IsNullOrEmpty(leave.UserName))
                {
                    var user = await _db.Users.FirstOrDefaultAsync(l => l.UserName == leave.UserName);
                    switch (leave.LeaveType)
                    {
                        case "Casual Leave":
                            user.AvailedCasualLeaves += leave.TotalDays;
                            break;
                        case "Sick Leave":
                            user.AvailedSickLeaves += leave.TotalDays;
                            break;
                        case "Optional Holiday":
                            user.AvailedoptionalHolidays += leave.TotalDays;
                            break;
                        case "Earned Leave":
                            user.AvailedEarnedLeaves += leave.TotalDays;
                            break;
                        default :
                            break;
                    }
                    await _db.SaveChangesAsync();
                }
                leave.LeaveId = count < 1 ? "LeR_00001" : "LeR_" + (count + 1).ToString("D5");
                leave.Status = "Pending";
                leave.TotalDays = (leave.EndDate - leave.StartDate).Days + 1;
                _db.LeavesModules.Add(leave);
                await _db.SaveChangesAsync();
                return "Leave Applied Successfully.";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<List<LeavesModule>?> GetLeavesList(string userName)
        {
            List<LeavesModule>? list = new List<LeavesModule>();
            try
            {
                if (userName != "" && await _db.LeavesModules.CountAsync() > 0)
                {
                    list = await _db.LeavesModules.Where(l => l.UserName == userName).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                return list = new List<LeavesModule>();
            }
            return list;
        }

        public async Task<string> CancelLeave(string leaveId)
        {
            try
            {
                var leave = await _db.LeavesModules.FirstOrDefaultAsync(l => l.LeaveId == leaveId);
                if (leave != null)
                {
                    leave.Status = "Cancelled";
                    if (!string.IsNullOrEmpty(leave.UserName))
                    {
                        var user = await _db.Users.FirstOrDefaultAsync(l => l.UserName == leave.UserName);
                        switch (leave.LeaveType)
                        {
                            case "Casual Leave":
                                user.AvailedCasualLeaves -= leave.TotalDays;
                                break;
                            case "Sick Leave":
                                user.AvailedSickLeaves -= leave.TotalDays;
                                break;
                            case "Optional Holiday":
                                user.AvailedoptionalHolidays -= leave.TotalDays;
                                break;
                            case "Earned Leave":
                                user.AvailedEarnedLeaves -= leave.TotalDays;
                                break;
                            default:
                                break;
                        }
                        await _db.SaveChangesAsync();
                    }
                    var result = await _db.SaveChangesAsync();

                    return "Leave Cancelled Successfully.";

                }
                else
                {
                    return $"Leave with LeavId '{leaveId}' not found";
                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }
        }

        public async Task<LaeveBalances> GetLeaveBalance(string userId)
        {
            LaeveBalances list = new LaeveBalances();
            try
            {
                var u = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userId);
                if (u != null) 
                {
                    list = new LaeveBalances
                    {
                        CasualTotal = u.TotalCasualLeaves.ToString(),
                        CasualAvailed = u.AvailedCasualLeaves.ToString(),
                        CasualBalance = (u.TotalCasualLeaves - u.AvailedCasualLeaves).ToString(),
                        SickBalance = (u.TotalSickLeaves - u.AvailedSickLeaves).ToString(),
                        OptionalBalance = (u.TotalOptionalHolidays - u.AvailedoptionalHolidays).ToString(),
                        EarnedBalance = (u.TotalEarnedLeaves - u.AvailedEarnedLeaves).ToString(),
                        SickTotal = u.TotalSickLeaves.ToString(),
                        SickAvailed = u.AvailedSickLeaves.ToString(),
                        OptionalTotal = u.TotalOptionalHolidays.ToString(),
                        OptionalAvailed = u.AvailedoptionalHolidays.ToString(),
                        EarnedTotal = u.TotalEarnedLeaves.ToString(),
                        EarnedAvailed = u.AvailedEarnedLeaves.ToString(),
                    };
                }
                
                   
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }

        public async Task<List<LeavesModule>> GetLeavesForApproval(string userid)
        {
            
            List<LeavesModule> list = new List<LeavesModule>();
            try
            {
                if (userid != "")
                {
                    var Rlist = new List<string>();
                    var user = await _db.Users.Where(u => u.UserName == userid)
                        .Include(u => u.DesignationId)
                        .FirstOrDefaultAsync();
                    if(user.DesignationId != null && user.DesignationId == "D001")
                    {
                        Rlist.Add("D002");
                        Rlist.Add("D003");
                        Rlist.Add("D005");
                    }else if (user.DesignationId != null && user.DesignationId == "D004")
                    {
                        Rlist = new List<string> { "D001"};
                    }else if (user.DesignationId != null && user.DesignationId == "D006")
                    {
                        Rlist = new List<string> { "D001", "D004" };
                    }

                    list = await _db.LeavesModules
                       .Join(_db.Users,
                       LeavesModules => LeavesModules.UserName,
                       Users => Users.UserName,
                       (LeavesModules, Users) => new { Users, LeavesModules }
                       )
                       .Where(joined => joined.Users.CollegeCode == user.CollegeCode && Rlist.Contains(joined.Users.DesignationId))
                       .Select(joined => joined.LeavesModules)
                       .ToListAsync();

                    return list;

                }
                else
                {
                    return list;
                }
            }
            catch (Exception ex)
            {
                return list;
            }
           
        }
        public async Task<string> ApproveLeave(string leaveId, string userId,string remarks,string flag)
        {
            try
            {
                var leave = await _db.LeavesModules.FirstOrDefaultAsync(l => l.LeaveId == leaveId);
                if (leave != null)
                {   leave.Remarks = remarks;
                    
                    if(flag == "R")
                    {
                        leave.Status = "Rejected";
                        if (!string.IsNullOrEmpty(leave.UserName))
                        {
                            var user = await _db.Users.FirstOrDefaultAsync(l => l.UserName == leave.UserName);
                            switch (leave.LeaveType)
                            {
                                case "Casual Leave":
                                    user.AvailedCasualLeaves -= leave.TotalDays;
                                    break;
                                case "Sick Leave":
                                    user.AvailedSickLeaves -= leave.TotalDays;
                                    break;
                                case "Optional Holiday":
                                    user.AvailedoptionalHolidays -= leave.TotalDays;
                                    break;
                                case "Earned Leave":
                                    user.AvailedEarnedLeaves -= leave.TotalDays;
                                    break;
                                default:
                                    break;
                            }
                            await _db.SaveChangesAsync();
                        }
                    }
                    else
                    {

                        leave.Status = "Approved";
                    }
                    var result = await _db.SaveChangesAsync();
                    var status = flag == "R" ? "Rejected" : "Approved";
                    return $"Leave {status} Successfully.";

                }
                else
                {
                    return $"Leave with LeavId '{leaveId}' not found";
                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }
        }

        public async Task<string> AddCollege(Colleges clg)
        {
            try
            {
                var count = await _db.Colleges.CountAsync();
                clg.CollegeCode = count < 1 ? "clg001" : "clg"+(count+1).ToString("D3");
                _db.Colleges.Add(clg);
                await _db.SaveChangesAsync();
                return "College Added Successfully.";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public async Task<string> AddDesignation(Designations clg)
        {
            try
            {
                var count = await _db.Designations.CountAsync();
                clg.DesignationId = count < 1 ? "D001" : "D"+(count+1).ToString("D3");
                _db.Designations.Add(clg);
                await _db.SaveChangesAsync();
                return "Designation Added Successfully.";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<List<Colleges>> GetCollegesList()
        {
            return await _db.Colleges.ToListAsync();
        }
        public async Task<List<Designations>> GetDesignationsList()
        {
            return await _db.Designations.ToListAsync();
        }

        public async Task<Profile> GetProfile(string userId)
        {
            Profile profile = new Profile();
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userId);
                if (user != null)
                {
                    profile.FullName = user.EmployeeName;
                    profile.Email = user.Email;
                    profile.Mobile = user.PhoneNumber;
                    profile.Aadhaar = user.AadhaarId;
                    profile.PAN = user.PanId;
                    profile.ProfilePhotoUrl = user.Passport_DocumentPath;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return profile;
        }

        public async Task<IdCardModel> getIdDetails(string userId)
        {
            IdCardModel id = new IdCardModel();
            try
            {
                var user = await _db.Users.Where(e => e.UserName == userId).Include(e => e.Designations).FirstOrDefaultAsync();
                id.EmployeeName = user.EmployeeName;
                id.EmployeeId = user.UserName;
                id.Role = user.Designations.DesignationName;
                id.PhotoUrl = user.Passport_DocumentPath;
                id.EmployeeSignatureUrl = user.Singnature_DocumentPath;
                id.ValidTill = DateTime.Today.ToString("2026-08-10");

            }
            catch (Exception)
            {

                throw;
            }
            return id;
        }

        public async Task<User> GetUserDetails(string UserName)
        {
            User user = new User();
            try
            {
                user = await _db.Users.Where(u => u.UserName == UserName).Include(u => u.Designations).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return user;
        }
    }
}

