using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTMS.DatabaseContext;
using CTMS.Models;
using static CTMS.Help.Helper;
using CTMS.Help;

namespace CTMS.Controllers
{
     [CheckSessionIsAvailable]
    [NoDirectAccess]
    public class RequestFormsController : Controller
    {
        private readonly CTMSDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public RequestFormsController(CTMSDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.GetString("UserName") == null)
            {
                TempData["msg"] = "Access Denied !! ";
                RedirectToAction("Login", "Account");
            }


        }

        // GET: RequestForms
        public async Task<IActionResult> Index()
        {
              return _context.RequestForms != null ? 
                          View(await _context.RequestForms.Where(x => x.Status == "New").ToListAsync()) :
                          Problem("Entity set 'CTMSDbContext.RequestForms'  is null.");
        }

        public IActionResult AssignedTasks()
        {
            var userrequest = _context.RequestForms.Where(x => x.Status == "On Process").ToList();
            return View(userrequest);
        }
        public IActionResult ResolvedTasks()
        {
            var userrequest = _context.RequestForms.Where(x => x.Status == "Resolved").ToList();
            return View(userrequest);
        }


        public async Task<IActionResult> AssignRequest(int id )
        {
            
            var appName = _context.ApplicationNames.ToList();
            var department = _context.Departments.ToList();
            var user = _context.Users.Where(x => x.RoleId == 2).ToList();
            ViewBag.appNameList = new SelectList(appName, "Name", "Name");
            ViewBag.departmentList = new SelectList(department, "Name", "Name");
            ViewBag.userList = new SelectList(user, "UserName", "UserName");
                var requestForm = await _context.RequestForms.FindAsync(id);
                if (requestForm == null)
                {
                    return NotFound();
                }
                return View(requestForm);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRequest(int id, RequestForm requestForm)
        {
            requestForm.Status = "On Process";
            requestForm.CreatedBy = _httpContext.HttpContext.Session.GetString("UserName");
            //if (ModelState.IsValid)
            //{
            if (id != 0)
            {
                try
                {
                    _context.Update(requestForm);
                    await _context.SaveChangesAsync();
                    TempData["msg"] = "Assigned Successfully !!";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!RequestFormExists(requestForm.Id))
                    {
                        TempData["msg"] = ex.ToString();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> AddOrEditRequest(int id = 0)
        {
            var appName = _context.ApplicationNames.ToList(); 
            ViewBag.appNameList = new SelectList(appName, "Name", "Name");
            RequestForm requestForm1 = new RequestForm();
            if(id != 0)
            {
                requestForm1 = await _context.RequestForms.Where(x => x.Id == id).FirstOrDefaultAsync();
                requestForm1.SelectedDepartment = requestForm1.Department.Split(", ").ToArray();
            }
            requestForm1.DepartmentCollection = await _context.Departments.ToListAsync();
            return View(requestForm1);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditRequest(int id,RequestForm requestForm)
        {
            requestForm.Department = string.Join(", ", requestForm.SelectedDepartment);
            requestForm.CreatedDate = DateTime.Now;
            requestForm.CreatedBy = _httpContext.HttpContext.Session.GetString("UserName");
            requestForm.ModifiedDate = DateTime.Now;
            requestForm.ModifiedBy = _httpContext.HttpContext.Session.GetString("UserName");
            requestForm.AssignedTo = _httpContext.HttpContext.Session.GetString("UserName");

            //requestForm.isSign = false;
            requestForm.SignApprove = 0;

            if (id == 0)
                {
                //requestForm.CreatedDate = DateTime.Now;
                //requestForm.CreatedBy = _httpContext.HttpContext.Session.GetString("UserName");
                var dbName = _context.ApplicationNames.Where(x => x.Name == requestForm.ApplicationName).FirstOrDefault();
                requestForm.DatabaseName = dbName.DatabaseName;

                await _context.RequestForms.AddAsync(requestForm);
                    await _context.SaveChangesAsync();
                TempData["msg"] = "Added Successfully";
            }
                else
                {
                    try
                    {
                    //requestForm.CreatedDate = DateTime.Now;
                    //requestForm.CreatedBy = _httpContext.HttpContext.Session.GetString("UserName");
                    _context.Update(requestForm);
                        await _context.SaveChangesAsync();
                    TempData["msg"] = "Updated Successfully";
                }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RequestFormExists(requestForm.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }  
                    }
                }
            
            return RedirectToAction("ResolvedTasks","User");

            }

        private bool RequestFormExists(int id)
        {
          return (_context.RequestForms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
