using Core;
using Data;
using Data.Repository;
using MVCTut.ViewModels;
using NHibernate;
using NHibernate.Linq;
using PagedList;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class UserController : Controller
    {
        private UserRepository _userRepository;

        //public HttpSessionState session { get; set; }
        public UserController()
        {
            _userRepository = new UserRepository();
        }
        //public ActionResult Index()
        //{
        //    try
        //    {
        //        return View(_userRepository.GetAll());
        //    }
        //    catch (Exception exception)
        //    {
        //        ViewBag.Exception = exception.Message;
        //    }

        //    return View(new User());
        //}
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var users = _userRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        users = session.Query<User>().Where(u => u.LastName.ToUpper().Contains(searchString.ToUpper()) ||
                        u.FirstName.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }

                    switch (sortOrder)
                    {
                        case "name_desc":
                            users = users.OrderByDescending(a => a.LastName).ToList();
                            break;
                        default:
                            users = users.OrderBy(u => u.Id).ToList();
                            break;
                    }
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    return View(users.ToPagedList(pageNumber, pageSize));
                }
                //return View(_userRepository.GetAll());
                return View(users.ToList());
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new User());
        }
        public ActionResult Create()
        {
            var repository = new Repository<Branch>();
            ViewBag.Branches = repository.GetAll();
            ViewBag.UserLevel = Enum.GetValues(typeof(UserLevel));

            return View(new User());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            ViewBag.UserLevel = Enum.GetValues(typeof(UserLevel));

            if (ModelState.IsValid)
            {
                try
                {
                    var users = _userRepository.Filter(u => u.Email == user.Email);
                    var username = _userRepository.Filter(u => u.UserName == user.UserName);
                    if (username.Count == 0)
                    {
                        if (users.Count == 0)
                        {
                            user.DateAdded = DateTime.Now;
                            user.DateUpdated = DateTime.Now;
                            user.Password = GeneratePassword();
                            _userRepository.Save(user);
                            SendLoginCredentials(user.Email, user.FirstName, user.UserName, user.Password);
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("Email", "This email already exists.");
                    }
                    ModelState.AddModelError("UserName", "This username already exists");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception.Message;
                }
            }

            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();

            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.UserLevel = Enum.GetValues(typeof(UserLevel));
            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();
            var userRepository = new Repository<User>();
            return View(userRepository.GetById(id));
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(User user, int id)
        {
            ViewBag.UserLevel = Enum.GetValues(typeof(UserLevel));
            if (ModelState.IsValid)
            {
                try
                {
                    var userRepository = new Repository<User>();
                    var oldEntity = userRepository.GetById(user.Id);
                    user.Password = oldEntity.Password;
                    user.DateAdded = oldEntity.DateAdded;
                    user.DateUpdated = DateTime.Now;
                    _userRepository.Update(user, user.Id);

                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }
            return View(user);
        }
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var userdetails = session.Query<User>().FirstOrDefault(i => i.UserName == user.UserName && i.Password == user.Password);

                        if (userdetails != null)
                        {
                            Session["User"] = userdetails;
                            ViewBag.Userbranch = userdetails.Branch;
                            return RedirectToAction("Index", "Home");

                        }
                        ModelState.AddModelError("", "Invalid Credentials");
                    }
                }

            }


            return View();
        }
        [HttpGet]
        public ActionResult ChangePassWord()
        {
            return View();
        }
        public ActionResult ChangePassWord(ChangePasswordViewModel changePass)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        // var userdetails = session.Query<User>().FirstOrDefault(i => i.UserName == user.UserName && i.Password == user.Password);
                        //if (userdetails != null)
                        //{
                        var userdetails = session.Query<User>().FirstOrDefault(i => i.Password == changePass.Password);
                        var userRepository = new Repository<User>();
                        if (changePass.NewPassword == changePass.ConfirmNewPassword)
                        {
                            userdetails.Password = changePass.NewPassword;
                            userRepository.Update(userdetails, userdetails.Id);
                            // Session["User"] = userdetails;
                            return RedirectToAction("Index", "Home");

                        }
                        ModelState.AddModelError("", "Invalid Credentials");
                    }
                }

            }


            return View();
        }
        public ActionResult LoggedInStatusMessage()
        {
            var profileData = this.Session["UserProfile"] as User;

            /* From here you could output profileData.FullName to a view and
            save yourself unnecessary database calls */
            return View(profileData.FullName);
        }

        public ActionResult Details(int id)
        {
            return View(_userRepository.GetById(id));
        }


        public ActionResult Delete(int id)
        {
            return View(_userRepository.GetById(id));

        }

        [HttpPost]
        public ActionResult Delete(int id, User user)
        {
            try
            {
                _userRepository.Delete(user);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return View();
            }
        }



        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        private string GeneratePassword()
        {
            //StringBuilder builder = new StringBuilder();
            return RandomString(8, false);
        }

        private string GenerateUsername(string firstname, string lastName)
        {
            string username = lastName.Substring(0, 1) + firstname;
            return username.ToLower();
        }

        private static void SendLoginCredentials(string user_email, string firstname, string username, string password)
        {
            //Send Mail
            //Create an object of MailMessage class
            MailMessage mail = new MailMessage();
            mail.To.Add(user_email);
            //Sender's address
            mail.From = new MailAddress("dan.onyeani@gmail.com");

            //Mail Subject
            mail.Subject = "Your CBA Login Credentials";

            //Mail Body
            string Body = "Hi " + firstname + "<br/></br/> Your CBA Login Details <br/><br/> Username : " + username + " Password : " + password;
            mail.Body = Body;

            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";

            smtp.Port = 587;

            smtp.Credentials = new System.Net.NetworkCredential
                 ("dan.onyeani@gmail.com", "daniel95");

            smtp.EnableSsl = true;

            smtp.Send(mail);

        }


    }


}