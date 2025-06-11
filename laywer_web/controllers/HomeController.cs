
using laywer_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Collections.Generic;

namespace laywer_web.controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult about()
        {
            return View();
        }
        public IActionResult service()
        {
            return View();
        }
        public IActionResult blog()
        {
            return View();
        }
        public IActionResult blogchild()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }
        private mycontext _Context;


        public HomeController(mycontext context)
        {
            _Context = context;

        }
        /*for dropdown*/


        [HttpPost]
        public IActionResult contact(contact user)
        {
            _Context.tbl_contact.Add(user);
            _Context.SaveChanges();
            return View();
        }
        /* for login*/
        public IActionResult user_login()
        {
            return View();
        }


        public IActionResult logineduser()
        {

            var notlogin = HttpContext.Session.GetString("user_session");
            if (string.IsNullOrEmpty(notlogin))
            {


                return View();
            }


            else
            {

                var logined = _Context.tbl_user.Where(s => s.user_id == int.Parse(notlogin)).ToList();
                lawyerViewModel mydata = new lawyerViewModel()
                { user_detail = logined };

                return View(mydata);

            }


        }
        [HttpPost]
        public IActionResult user_login(string userEmail, string userPassword)
        {
            var data = _Context.tbl_user.FirstOrDefault(e => e.user_email == userEmail);
            if (data != null && data.user_password == userPassword )
            {
                if(data.status == 1)
                {
                    HttpContext.Session.SetString("user_session", data.user_id.ToString());
                    return RedirectToAction("laywersdetails");
                }
                else
                {
                    return RedirectToAction("waitedPage");

                }
            }
            else
            {
                ViewBag.message = "Incorrect Password";
                TempData["alertmassage"] = "email or password is incorrected";
                return View();
            }


        }



        /*for register*/
        public IActionResult user_register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult user_register(user userdetails, string user_email)
        {

            _Context.tbl_user.Add(userdetails);
            _Context.SaveChanges();

            return RedirectToAction("user_login");
        }


        /* for log out*/
        public IActionResult logout()
        {
            HttpContext.Session.Remove("user_session");
            return RedirectToAction("user_login");

        }
        public IActionResult laywersdetails()
        {

            var notlogin = HttpContext.Session.GetString("user_session");
            if (notlogin != null)
            {

                var lawyerData = _Context.tbL_lawyersdetail.ToList();

                var logined = _Context.tbl_user.Where
                    (s => s.user_id == int.Parse(notlogin)).ToList();
                lawyerViewModel mydata = new lawyerViewModel()
                {
                    user_detail = logined,
                    lawyersdetails = lawyerData

                };

                return View(mydata);

            }


            else
            {

                return RedirectToAction("user_login");
            }

        }
        /*for sreach the lawyer*/
        /*to sreach tha data*/
        [HttpGet]

        public IActionResult laywersdetails(string search_text)
        {

            var notlogin = HttpContext.Session.GetString("user_session");

            if (notlogin != null)
            {


                /*we create the object with the help of model*/
                List<lawyersdetail> lawyerData = new List<lawyersdetail>();
                if (string.IsNullOrEmpty(search_text))
                {
                    lawyerData = _Context.tbL_lawyersdetail.ToList();



                }

                else
                {
                    lawyerData = _Context.tbL_lawyersdetail.FromSqlInterpolated(

                   $"select * from tbl_lawyersdetail where lawyer_name={search_text} or lawyer_dealed={search_text}")
                        .
                   ToList();



                }
                if (lawyerData.Count == 0)
                {
                    TempData["error"] = $"record not found {search_text} ";

                }

                var adminRecords = _Context.tbl_user.Where(e => e.user_id == int.Parse(notlogin)).ToList();
                lawyerViewModel mydata = new lawyerViewModel()
                {
                    user_detail = adminRecords,
                    lawyersdetails = lawyerData,
                };
                return View(mydata);




            }
            else
            {
                return RedirectToAction("user_login");
            }

        }

        public IActionResult lawyer_profile(int id)
        {

            var laywerDetail = _Context.tbL_lawyersdetail.FirstOrDefault(u => u.lawyer_id == id);
            lawyerViewModel mydata = new lawyerViewModel()
            {

                lawyersdetail = laywerDetail,
            };
            return View(mydata);
        }
        public IActionResult get_appoint(int id)
        {
            var notlogined = HttpContext.Session.GetString("user_session");
            if (notlogined != null)
            {
                var laywerDetail = _Context.tbL_lawyersdetail.FirstOrDefault(u => u.lawyer_id == id);
                var logined = _Context.tbl_user.Where
                    (s => s.user_id == int.Parse(notlogined)).ToList();

                ;

                lawyerViewModel mydata = new lawyerViewModel()
                {

                    lawyersdetail = laywerDetail,
                    user_detail = logined

                };
                return View(mydata);


            }

            else
            {
                return RedirectToAction("user_login");
            }
        }

        /*for actual add up*/
        [HttpPost]
        public IActionResult get_appoint(int id, bookeduser get_appoitment)
        {
            var notlogined = HttpContext.Session.GetString("user_session");
            if (notlogined != null)
            {
                _Context.tbl_bokedappointment.Add(get_appoitment);
                _Context.SaveChanges();
                var laywerDetail = _Context.tbL_lawyersdetail.FirstOrDefault(u => u.lawyer_id == id);
                var logined = _Context.tbl_user.Where
                  (s => s.user_id == int.Parse(notlogined)).ToList();

                ;

                lawyerViewModel mydata = new lawyerViewModel()
                {

                    lawyersdetail = laywerDetail,
                    user_detail = logined

                };
                return View(mydata);


            }

            else
            {
                return RedirectToAction("user_login");
            }

        }
        public IActionResult view_appoint()
        {
            return View();
        }

        [HttpGet]

        public IActionResult view_appoint(string search_text)
        {

            var notlogin = HttpContext.Session.GetString("user_session");

            if (notlogin != null)
            {


                /*we create the object with the help of model*/
                List<bookeduser> lawyerData = new List<bookeduser>();
                if (string.IsNullOrEmpty(search_text))
                {
                  
                }

                else
                {
                    lawyerData = _Context.tbl_bokedappointment.FromSqlInterpolated(

                   $"select * from  tbl_bokedappointment where booked_username ={search_text} or Booked_useremail={search_text}" )
                        .
                   ToList();
                    
                    



                }
                if (lawyerData.Count == 0)
                {
                    TempData["error"] = $" Any record does  not found {search_text} ";

                }
               
               
                
               

                var adminRecords = _Context.tbl_user.Where(e => e.user_id == int.Parse(notlogin)).ToList();
                lawyerViewModel mydata = new lawyerViewModel()
                {
                    user_detail = adminRecords,
                    get_appointments = lawyerData,
                };
                return View(mydata);




            }
            else
            {
                return RedirectToAction("user_login");
            }

        }


    }


}
