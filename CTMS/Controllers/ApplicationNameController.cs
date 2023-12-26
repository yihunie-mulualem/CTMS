
using CTMS.DatabaseContext;
using CTMS.Help;
using CTMS.Models;
using Microsoft.AspNetCore.Mvc;
using static CTMS.Help.Helper;

namespace CTMS.Controllers
{
    [CheckSessionIsAvailable]
    [NoDirectAccess]
    public class ApplicationNameController : Controller
    {
        private readonly CTMSDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ApplicationNameController(CTMSDbContext context,IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.GetString("UserName") == null)
            {
                TempData["msg"] = "Access Denied !! ";
                RedirectToAction("Login", "Account");
            }

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ApplicationRegister()
        {
            return View();
        }
        public IActionResult ApplicationName(ApplicationName Application)
        {
            if (ModelState.IsValid)
            {
                _context.ApplicationNames.Add(Application);
                _context.SaveChanges(true);
                TempData["AlertMessage"]="Application Created successfully";
                return RedirectToAction(nameof(ApplicationView));
            }
            return RedirectToAction(nameof(ApplicationView));
        }
        public IActionResult ApplicationView()
        {

            var Application = _context.ApplicationNames.ToList().Where(Application => Application.viewStatus == true).OrderBy(e => e.Name); 

            return View(Application);
        }
        private ApplicationName GetApplicationByID(int id)
        {
            var Application = _context.ApplicationNames.FirstOrDefault(x => x.Id == id);
            return Application;
        }
  
        public IActionResult ApplicationEdit(int id)
        {
            var application = GetApplicationByID(id);
            if (application == null)
            {
                return RedirectToAction(nameof(ApplicationView));
            }
            return View(application);
        }
        public IActionResult ApplicationUpdate(ApplicationName Application)
        {
            if (ModelState.IsValid)
            {
                _context.ApplicationNames.Update(Application);
                _context.SaveChanges(true);
                TempData["AlertMessage"] = "Application Updated successfully";
                return RedirectToAction(nameof(ApplicationView));
            }
            return RedirectToAction(nameof(ApplicationRegister));
        }
        public IActionResult ApplicationDelete(int id)
        {
            var application = GetApplicationByID(id);

            application.viewStatus = false;
            _context.Update(application);
            TempData["AlertMessage"] = "Application deleted successfully";

            _context.SaveChanges();

            if (application == null)
            {
                return RedirectToAction(nameof(ApplicationView));
            }
            return RedirectToAction(nameof(ApplicationView));
        }
    }
}
