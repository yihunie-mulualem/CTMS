using CTMS.DatabaseContext;
using CTMS.Help;
using CTMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using static CTMS.Help.Helper;

namespace CTMS.Controllers
{
    
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly CTMSDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public AccountController(CTMSDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public IActionResult AddUser()
        {
            return View();
        }
        public IActionResult UserRegister(User user)

        {
            var user2 = new User
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId
            };

            string pass = user.Password;
            string encryptPass = EncodePasswordToBase64(pass);
            user2.Password = encryptPass;
            _context.Users.Add(user2);
            _context.SaveChanges(true);
            return RedirectToAction(nameof(UserView));

        }
        public IActionResult UserView()
        {
            var user = _context.Users.ToList().Where(User => User.viewStatus == true).OrderBy(e => e.UserName);

            return View(user);
        }
        private User GetUserByID(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }
        public IActionResult UpdateUser(int id)
        {
            var user = GetUserByID(id);
            if (user == null)
            {
                return RedirectToAction(nameof(UserView));
            }
            string PasswordDecode = DecodeFrom64(user.Password);
            user.Password = PasswordDecode;
            return View(user);
        }
        public IActionResult UserUpdate(User user)
        {


            string pass = user.Password;
            string encryptPass = EncodePasswordToBase64(pass);
            user.Password = encryptPass;
            _context.Users.Update(user);
            _context.SaveChanges(true);
            return RedirectToAction(nameof(UserView));
        }
        public IActionResult UserDelete(int id)
        {
            var user = GetUserByID(id);

            user.viewStatus = false;
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(UserView));
        }

        public IActionResult Login()
        {
            _httpContext.HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult LoginAction(Login login)
        {

            if (ModelState.IsValid)
            {
                string password = EncodePasswordToBase64(login.Password);
                var users = _context.Users.Where(a => a.UserName.Equals(login.UserName) && a.Password.Equals(password) && a.viewStatus == true).FirstOrDefault();
              
                if (users != null)
                {
                    var user = _context.Users.Where(x => x.UserName == login.UserName).FirstOrDefault();
                    _httpContext.HttpContext.Session.SetString("UserName", user.UserName);
                    _httpContext.HttpContext.Session.SetInt32("UserRole", user.RoleId);
                    if (user.RoleId == 2)
                    {
                        TempData["msg"] = "Access Guaranteed !! ";
                        return RedirectToAction("ResolvedTasks", "User");
                    }
                    else if (user.RoleId == 3 || user.RoleId == 1)
                    {
                        //TempData["AlertMessage"] = "UserName or Password Incorrect";
                        TempData["msg"] = "Access Guaranteed !! ";
                        return RedirectToAction("appdbchanges", "User");
                    }
                }
                else
                {
                    TempData["AlertMessage"] = "UserName or Password Incorrect";
                    return RedirectToAction(nameof(Login));
                }
          
            }
              //TempData["msg"] = "Access Denied !! ";
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
            string username = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(username))
            {
                _httpContext.HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult LoginView()
        {
            return RedirectToAction("Login", "Account");
        }
        //this function Convert to Encord your Password
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        public async Task<IActionResult> ChangePassword()
        {
            var user1 = _httpContext.HttpContext.Session.GetString("UserName");
            var userdetail = _context.Users.Where(x => x.UserName == user1).FirstOrDefault();
            return View(userdetail);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(User user)
        {
            //if(ModelState.IsValid)
            //  {
            string passwd = EncodePasswordToBase64(user.Password);
            string newpasswd = EncodePasswordToBase64(user.NewPassword);
            string confiermpasswd = EncodePasswordToBase64(user.ConfiermPassword);

            var userupdate = _context.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            if (userupdate.Password == passwd)
            {
                if (newpasswd == confiermpasswd)
                {
                    userupdate.Password = newpasswd;
                    _context.Users.Update(userupdate);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["msg"] = " Confierm Password Not Match!";
                    return View();
                }

            }
            else
            {
                TempData["msg"] = " Current Password Not Correct!";
                return View();
            }



            //}

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}
