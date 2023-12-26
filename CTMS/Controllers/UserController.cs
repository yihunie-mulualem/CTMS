using CTMS.DatabaseContext;
using CTMS.Help;
using CTMS.Models;
using CTMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using static CTMS.Help.Helper;
namespace CTMS.Controllers
{
    [CheckSessionIsAvailable]
    [NoDirectAccess]
    public class UserController : Controller
    {
        private readonly CTMSDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _environment;
        public UserController(CTMSDbContext context, IWebHostEnvironment environment, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            _environment = environment;


            if (_httpContext.HttpContext.Session.GetString("UserName") == null)
            {
                RedirectToAction("Login", "Account");
            }
        }

        
        // For Showing Request Status
        public IActionResult Index()
        {
            var username = _httpContext.HttpContext.Session.GetString("UserName");
            var userrequest = _context.RequestForms.Where(x => x.AssignedTo == username && x.Status == "On Process").ToList();
            return View(userrequest);
        }
        public IActionResult ResolvedTasks()
        {
            var username = _httpContext.HttpContext.Session.GetString("UserName");
            var userrequest = _context.RequestForms.Where(x => x.AssignedTo == username && x.Status == "Resolved").ToList();
            return View(userrequest);
        }
        public IActionResult AssignedToTeam()
        {
            var userrequest = _context.RequestForms.Where(x => x.Status == "On Process").ToList();
            return View(userrequest);
        }

        // truck All Changes
        public async Task<IActionResult> ApplicationChanges()
        {
            var changeDetail = new List<ApplicationViewModels>();
            // var username = _httpContext.HttpContext.Session.GetString("UserName");

            changeDetail = (from rf in _context.RequestForms
                            join app in _context.ApplicationCases on rf.Id equals app.RequestForm.Id
                            select new ApplicationViewModels
                            {
                                ApplicationName = rf.ApplicationName,
                                PageName = app.PageName,
                                Description = app.Description,
                                AssignedTo = rf.AssignedTo,
                                ModifiedBy = rf.ModifiedBy,
                                ModifiedDate = rf.ModifiedDate

                            }).ToList();
            return View(changeDetail);
        }

        public async Task<IActionResult> DatabaseChanges()
        {
            var changeDetail = new List<DatabaseViewModel>();
            // var username = _httpContext.HttpContext.Session.GetString("UserName");

            changeDetail = (from rf in _context.RequestForms
                            join db in _context.DatabaseCases on rf.Id equals db.RequestForm.Id
                            select new DatabaseViewModel
                            {
                                DatabaseName = rf.DatabaseName,
                                TableName = db.TableName,
                                // FieldName = db.FieldName,
                                Description = db.Description,
                                AssignedTo = rf.AssignedTo,
                                ModifiedBy = rf.ModifiedBy,
                                ModifiedDate = rf.ModifiedDate

                            }).ToList();
            return View(changeDetail);
        }
        public async Task<IActionResult> AppDbChanges()
        {

            var appName = _context.ApplicationNames.ToList();
            ViewBag.appNameList = new SelectList(appName, "Name", "Name");

            var changeDetail = new List<App_Db_ViewModel>();
            // var username = _httpContext.HttpContext.Session.GetString("UserName");

            changeDetail = (from rf in _context.RequestForms
                                //join db in _context.DatabaseCases on rf.Id equals db.RequestForm.Id
                                //join app in _context.ApplicationCases on rf.Id equals app.RequestForm.Id
                            select new App_Db_ViewModel
                            {
                                RequestId = rf.Id,
                                ApplicationName = rf.ApplicationName,
                                ApplicationVersion = rf.ApplicationVersion,
                                // PageName = app.PageName,
                                // AppDescription = app.Description,
                                DatabaseName = rf.DatabaseName,
                                //TableName = db.TableName,
                                // FieldName = db.FieldName,
                                // DbDescription = db.Description,
                                Description = rf.Description,
                                AssignedTo = rf.AssignedTo,
                                ModifiedBy = rf.ModifiedBy,
                                ModifiedDate = rf.ModifiedDate,
                                SignApprove=rf.SignApprove,
                            }).ToList();
            return View(changeDetail);
        }
        // Posting New Changes
        [HttpPost]
        public async Task<IActionResult> AppDbChanges(string appName)
        {

            var listofApp = _context.ApplicationNames.ToList();
            ViewBag.appNameList = new SelectList(listofApp, "Name", "Name");

            var changeDetail = new List<App_Db_ViewModel>();
            // var username = _httpContext.HttpContext.Session.GetString("UserName");
            if (appName == null)
            {
                changeDetail = (from rf in _context.RequestForms
                                    //join db in _context.DatabaseCases on rf.Id equals db.RequestForm.Id
                                    //join app in _context.ApplicationCases on rf.Id equals app.RequestForm.Id
                                select new App_Db_ViewModel
                                {
                                    RequestId = rf.Id,
                                    ApplicationName = rf.ApplicationName,
                                    ApplicationVersion = rf.ApplicationVersion,
                                    // PageName = app.PageName,
                                    // AppDescription = app.Description,
                                    DatabaseName = rf.DatabaseName,
                                    //TableName = db.TableName,
                                    // FieldName = db.FieldName,
                                    // DbDescription = db.Description,
                                    Description = rf.Description,
                                    AssignedTo = rf.AssignedTo,
                                    ModifiedBy = rf.ModifiedBy,
                                    ModifiedDate = rf.ModifiedDate,
                                    //
                                    SignApprove=rf.SignApprove,
                                }).ToList();
            }
            else
            {
                changeDetail = (from rf in _context.RequestForms
                                    //join db in _context.DatabaseCases on rf.Id equals db.RequestForm.Id
                                    //join app in _context.ApplicationCases on rf.Id equals app.RequestForm.Id
                                select new App_Db_ViewModel
                                {
                                    RequestId = rf.Id,
                                    ApplicationName = rf.ApplicationName,
                                    ApplicationVersion = rf.ApplicationVersion,
                                    // PageName = app.PageName,
                                    // AppDescription = app.Description,
                                    DatabaseName = rf.DatabaseName,
                                    //TableName = db.TableName,
                                    // FieldName = db.FieldName,
                                    // DbDescription = db.Description,
                                    Description = rf.Description,
                                    AssignedTo = rf.AssignedTo,
                                    ModifiedBy = rf.ModifiedBy,
                                    ModifiedDate = rf.ModifiedDate,
                                    //
                                    SignApprove = rf.SignApprove,

                                }).Where(x => x.ApplicationName == appName).ToList();

            }


            return PartialView("_DataTablePartialView", changeDetail);
        }
        public IActionResult GenerateEntireReport(string appName)
        {

            TempData["msg"] = "rpt";
            if (appName != null)
            {
                TempData["app"] = appName;
            }
            else
            {
                TempData["app"] = null;
            }

            return RedirectToAction("AppDbChanges");

        }    
       public IActionResult GeneratReportById(int id)
        {

            TempData["msg"] = "sign_rpt";
            if (id != 0)
            {
                TempData["signId"] = id;
            }
            else
            {
                TempData["signId"] = 0;
            }

            return RedirectToAction("AppDbChanges");

        }
        public IActionResult RequestResponse(int id)
{
    var userrequest = _context.RequestForms.FirstOrDefault(x => x.Id == id);
    var viewData = new App_Db_ViewModel
    {
        RequestId = id,
        ApplicationName = userrequest.ApplicationName,
        DatabaseName = userrequest.DatabaseName
    };

    return View(viewData);
}
[HttpPost]
public async Task<IActionResult> RequestResponse(App_Db_ViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        var requestForm = _context.RequestForms.Where(c => c.Id == viewModel.RequestId).FirstOrDefault();
        try
        {


            if (viewModel.ApplicationName != null && viewModel.PageName != null && viewModel.AppDescription != null &&
                viewModel.DatabaseName != null && viewModel.TableName != null && viewModel.DbDescription != null &&
                viewModel.SourceCodeFile != null && viewModel.DatabaseSchemaFile != null)
            {
                requestForm.Status = "Resolved";
                requestForm.ChangeType = "App-Db Change";
                requestForm.ModifiedDate = DateTime.Now;
                requestForm.ModifiedBy = _httpContext.HttpContext.Session.GetString("UserName");






                //if (ModelState.IsValid)
                //{
                string srcFileExtension = string.Empty;
                string srcFileName = string.Empty;
                string DbFileName = string.Empty;
                string DbFileExtension = string.Empty;
                //string uploadsFolder = "F:\\
                //
                //";

                if (viewModel.SourceCodeFile != null || viewModel.DatabaseSchemaFile != null)
                {
                    srcFileName = Path.GetFileName(viewModel.SourceCodeFile.FileName);
                    srcFileExtension = Path.GetExtension(srcFileName);
                    DbFileName = Path.GetFileName(viewModel.DatabaseSchemaFile.FileName);
                    DbFileExtension = Path.GetExtension(DbFileName);

                    if (srcFileExtension == ".zip" || srcFileExtension == ".rar" && DbFileExtension == ".zip" || DbFileExtension == ".rar")
                    {
                        string uploadsFolderPath = Path.Combine(_environment.WebRootPath, "files");
                        string uniqueSrcFileName = srcFileName;
                        string uniqueDbFileName = DbFileName;
                        string srcFilePath = Path.Combine(uploadsFolderPath, uniqueSrcFileName);
                        string dbFilePath = Path.Combine(uploadsFolderPath, uniqueDbFileName);
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        using (var srcFileStream = new FileStream(srcFilePath, FileMode.Create))
                        {
                            await viewModel.SourceCodeFile.CopyToAsync(srcFileStream);
                        }
                        using (var dbFileStream = new FileStream(dbFilePath, FileMode.Create))
                        {
                            await viewModel.DatabaseSchemaFile.CopyToAsync(dbFileStream);
                        }



                        var AppData = new ApplicationCase
                        {
                            ApplicationName = viewModel.ApplicationName,
                            PageName = viewModel.PageName,
                            Description = viewModel.AppDescription,
                            SourceCodeFileName = uniqueSrcFileName,
                            SourceCodeFileURL = srcFilePath,
                            RequestForm = requestForm,

                        };
                        _context.ApplicationCases.Add(AppData);
                        _context.SaveChanges();

                        var dbData = new DatabaseCase
                        {
                            DatabaseName = viewModel.DatabaseName,
                            TableName = viewModel.TableName,
                            // FieldName = viewModel.FieldName,
                            Description = viewModel.DbDescription,
                            DatabaseSchemaFileName = uniqueDbFileName,
                            DatabaseSchemaFileURL = dbFilePath,
                            RequestForm = requestForm
                        };
                        _context.DatabaseCases.Add(dbData);
                        _context.SaveChanges();
                    }
                    else
                    {
                        TempData["msg"] = "File extension should be of either .zip or .rar format";
                        //TempData["MessageTitle"] = "Failed!";
                        TempData["msg"] = "error";
                        return RedirectToAction("Index");
                        //return BadRequest("File extension should be of either .zip or .rar format ");
                    }
                }
                else
                {
                    TempData["Message"] = "Please choose file to be saved";
                    // TempData["MessageTitle"] = "Failed!";
                    TempData["msg"] = "error";
                    return RedirectToAction("Index");
                    //return BadRequest("File not Selected");
                }


            }
            else
            if (viewModel.ApplicationName != null && viewModel.PageName != null && viewModel.AppDescription != null)
            {
                requestForm.Status = "Resolved";
                requestForm.ChangeType = "App Change";
                requestForm.ModifiedDate = DateTime.Now;
                requestForm.ModifiedBy = _httpContext.HttpContext.Session.GetString("UserName");

                //{
                string srcFileExtension = string.Empty;
                string srcFileName = string.Empty;
                //string uploadsFolder = "F:\\CTMS";

                if (viewModel.SourceCodeFile != null)
                {
                    srcFileName = Path.GetFileName(viewModel.SourceCodeFile.FileName);
                    srcFileExtension = Path.GetExtension(srcFileName);

                    if (srcFileExtension == ".zip" || srcFileExtension == ".rar")
                    {
                        string uploadsFolderPath = Path.Combine(_environment.WebRootPath, "files");
                        string uniqueSrcFileName = srcFileName;
                        string srcFilePath = Path.Combine(uploadsFolderPath, uniqueSrcFileName);

                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        using (var srcFileStream = new FileStream(srcFilePath, FileMode.Create))
                        {
                            await viewModel.SourceCodeFile.CopyToAsync(srcFileStream);
                        }

                        var AppData = new ApplicationCase
                        {
                            ApplicationName = viewModel.ApplicationName,
                            PageName = viewModel.PageName,
                            Description = viewModel.AppDescription,
                            SourceCodeFileName = uniqueSrcFileName,
                            SourceCodeFileURL = srcFilePath,
                            RequestForm = requestForm,
                        };
                        _context.ApplicationCases.Add(AppData);
                        _context.SaveChanges();
                    }
                    else
                    {
                        TempData["msg"] = "File extension should be of either .zip or .rar format";
                        //TempData["MessageTitle"] = "Failed!";
                        TempData["msg"] = "error";
                        return RedirectToAction("Index");
                        //return BadRequest("File extension should be of either .zip or .rar format ");
                    }
                }
                else
                {
                    TempData["Message"] = "Please choose file to be saved";
                    // TempData["MessageTitle"] = "Failed!";
                    TempData["msg"] = "error";
                    return RedirectToAction("Index");
                    //return BadRequest("File not Selected");
                }



            }
            else
            if (viewModel.DatabaseName != null && viewModel.TableName != null && viewModel.DbDescription != null)
            {
                requestForm.Status = "Resolved";
                requestForm.ChangeType = "Db Change";
                requestForm.ModifiedDate = DateTime.Now;
                requestForm.ModifiedBy = _httpContext.HttpContext.Session.GetString("UserName");




                string DbFileName = string.Empty;
                string DbFileExtension = string.Empty;
                //string uploadsFolder = "F:\\CTMS";

                if (viewModel.DatabaseSchemaFile != null)
                {
                    DbFileName = Path.GetFileName(viewModel.DatabaseSchemaFile.FileName);
                    DbFileExtension = Path.GetExtension(DbFileName);

                    if (DbFileExtension == ".zip" || DbFileExtension == ".rar")
                    {
                        string uploadsFolderPath = Path.Combine(_environment.WebRootPath, "files");
                        string uniqueDbFileName = DbFileName;
                        string dbFilePath = Path.Combine(uploadsFolderPath, uniqueDbFileName);
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        using (var dbFileStream = new FileStream(dbFilePath, FileMode.Create))
                        {
                            await viewModel.DatabaseSchemaFile.CopyToAsync(dbFileStream);
                        }


                        var dbData = new DatabaseCase
                        {
                            DatabaseName = viewModel.DatabaseName,
                            TableName = viewModel.TableName,
                            // FieldName = viewModel.FieldName,
                            Description = viewModel.DbDescription,
                            DatabaseSchemaFileName = uniqueDbFileName,
                            DatabaseSchemaFileURL = dbFilePath,
                            RequestForm = requestForm
                        };
                        _context.DatabaseCases.Add(dbData);
                        _context.SaveChanges();
                    }
                    else
                    {
                        TempData["msg"] = "File extension should be of either .zip or .rar format";
                        //TempData["MessageTitle"] = "Failed!";
                        TempData["msg"] = "error";
                        return RedirectToAction("Index");
                        //return BadRequest("File extension should be of either .zip or .rar format ");
                    }
                }
                else
                {
                    TempData["Message"] = "Please choose file to be saved";
                    // TempData["MessageTitle"] = "Failed!";
                    TempData["msg"] = "error";
                    return RedirectToAction("Index");
                    //return BadRequest("File not Selected");
                }


            }
            else
            {
                return BadRequest("Fill All Credential for Selected Change/Update Type");
            }
        }
        catch (Exception ex)
        {
            TempData["msg"] = ex.ToString();

            return RedirectToAction("Index");
        }

        finally
        {
            requestForm.ApplicationVersion = viewModel.ApplicationVersion;
            _context.SaveChanges();
        }

        TempData["msg"] = "message";
        return RedirectToAction("Index");
    }
    else
    {
        return BadRequest(ModelState);
    }
}
//---------------------------------------------------------------------------------------------//
public async Task<IActionResult> ResponseDetail(int id)
{
    // var userrequest = _context.RequestForms.FirstOrDefault(x => x.Id == id);
    var changeDetail = (from rf in _context.RequestForms
                        join db in _context.DatabaseCases on rf.Id equals db.RequestForm.Id
                        join app in _context.ApplicationCases on rf.Id equals app.RequestForm.Id
                        select new App_Db_ViewModel
                        {
                            RequestId = rf.Id,
                            ApplicationName = rf.ApplicationName,
                            PageName = app.PageName,
                            AppDescription = app.Description,
                            DatabaseName = rf.DatabaseName,
                            TableName = db.TableName,
                            // FieldName = db.FieldName,
                            DbDescription = db.Description,
                            AssignedTo = rf.AssignedTo,
                            ModifiedBy = rf.ModifiedBy,
                            ModifiedDate = rf.ModifiedDate,
                             SignApprove = rf.SignApprove,

                        }).FirstOrDefaultAsync(x => x.Id == id);
    return View(changeDetail);
}
        // ActionListener
        [HttpGet]
        public ActionResult SignIndicator(int id)
        {
            RequestForm change= _context.RequestForms.Find(id);
       
          if (change.SignApprove == 0)
            {
                change.SignApprove = 1;
                _context.RequestForms.Update(change);
                _context.SaveChanges();
            }
       
            return RedirectToAction("Index");
        }

    }
}
