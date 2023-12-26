using CTMS.DatabaseContext;
using CTMS.Help;
using CTMS.Models;
using Microsoft.AspNetCore.Mvc;
using static CTMS.Help.Helper;

namespace CTMS.Controllers
{
    [CheckSessionIsAvailable]
    [NoDirectAccess]
    public class DepartmentController : Controller
    {
        private readonly CTMSDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public DepartmentController(CTMSDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.GetString("UserName") == null)
            {

                RedirectToAction("Login", "Account");
            }


        }
        public IActionResult DepartmentCreation()
        {
            return View();
        }
        public IActionResult DepartmentRegister(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                _context.SaveChanges(true);
                TempData["AlertMessage"] = "Department Added successfully";

                return RedirectToAction(nameof(DepartmentView));
            }
            return RedirectToAction(nameof(DepartmentView));
        }
        public IActionResult DepartmentView()
        {
            var department = _context.Departments.ToList().Where(department => department.viewStatus == true).OrderBy(e => e.Name); 
            return View(department);
        }
        private Department GetDepartmentByID(int id)
        {
            var department = _context.Departments.FirstOrDefault(x => x.Id == id);
            return department;
        }
        public IActionResult DepartmentEdit(int id)
        {
            var Department = GetDepartmentByID(id);
            if (Department == null)
            {
                return RedirectToAction(nameof(Department));
            }
            return View(Department);
        }
        public IActionResult DepartmentUpdate(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges(true);
                TempData["AlertMessage"] = "Department Updated successfully";
                return RedirectToAction(nameof(DepartmentView));
            }
            return RedirectToAction(nameof(DepartmentView));
        }
        public IActionResult DepartmentDelete(int id)
        {
            var department = GetDepartmentByID(id);

            department.viewStatus = false;
            _context.Update(department);
            TempData["AlertMessage"] = "Department Deleted successfully";

            _context.SaveChanges();

            if (department == null)
            {
                return RedirectToAction(nameof(DepartmentView));
            }
            return RedirectToAction(nameof(DepartmentView));
        }

    }
}
