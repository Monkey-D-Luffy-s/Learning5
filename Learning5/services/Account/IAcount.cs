using Learning5.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace Learning5.services.Account
{
    public interface IAcount
    {
        Task<string> RegisterUser(User user);
        Task<Profile> GetProfile(string userId);
        Task<string> DeleteUser(string userid);
        Task<List<User>> GetEmployeesList();
        Task<string> Login(LoginUser user);
        Task<string> Logout();
        Task<string> AddRole(Roles role);
        Task<string> AssignRole(AssignRoleModel obj);
        Task<List<Roles>> GetRoles();
        Task<List<SelectListItem>> GetRolesForDropdown();
        Task<List<SelectListItem>> GetCollegesForDropdown();
        Task<List<SelectListItem>> GetEmployeesForUsersDropdown(string username);
        Task<List<SelectListItem>> GetEmployeesForAdminDropdown();
        Task<string> ApplyLeave(LeavesModule leave);
        Task<List<LeavesModule>> GetLeavesList(string userName);
        Task<List<LeavesListforApproveModel>> GetLeavesForApproval(string userid);
        Task<string> CancelLeave(string leaveId);
        Task<List<SelectListItem>> GetDesignationsForDropdown();
        Task<string> ApproveLeave(string leaveId, string userId, string remarks, string flag);
        Task<LaeveBalances> GetLeaveBalance(string userId);

        Task<IdCardModel> getIdDetails(string userId);
        Task<string> AddCollege(Colleges clg);
        Task<List<Colleges>> GetCollegesList();

        Task<User> GetUserDetails(string UserName);
        Task<string> AddDesignation(Designations clg);
        Task<List<Designations>> GetDesignationsList();

        Task<string> FillTimeSheet(TimeSheet timesheet);

        Task<List<string>> GetListofTimeshetFilled(string userid);

        Task<JsonResult> GetHolidays(int? year, int? month);
        Task<JsonResult> GetHolidaysList();
        Task<string> AddHoliday(string date, string name);

        Task<JsonResult> GetEmployeeTimeSheets(string userid);

        Task<string> ApproveTimesheet(string timesheetId);
    }
}
