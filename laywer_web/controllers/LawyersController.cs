using laywer_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace laywer_web.controllers
{
    public class LawyersController : Controller
    {
        private mycontext contexts;
        public LawyersController(mycontext context)
        {
            contexts = context;
        }
        public IActionResult Index()
        {
            var notlogined = HttpContext.Session.GetString("lawyer_session");
            if(notlogined!= null)
            {
                var Lawyer_Records = contexts.tbL_lawyersdetail.Where(e => e.lawyer_id == int.
                Parse(notlogined)).ToList();
                var counted_appointment = contexts.tbl_bokedappointment.
               Where(law => law.lawyer_id == int.Parse(notlogined)).Count();
                ViewBag.countappointment = counted_appointment;
                lawyerViewModel lawyer_data = new lawyerViewModel()
                { lawyersdetails = Lawyer_Records };

                return View(lawyer_data);
            }
            else
            {
                return RedirectToAction("loginfrom_lawyer");
            }
           
        }
        /*lawyer_profile*/
        public IActionResult lawyerProfile()
        {
            var notlogined = HttpContext.Session.GetString("lawyer_session");
            if (notlogined != null)
            {
              
                var Lawyer_Records = contexts.tbL_lawyersdetail.Where(e => e.lawyer_id == int.
                Parse(notlogined)).ToList();
                lawyerViewModel lawyer_data = new lawyerViewModel()
                { lawyersdetails = Lawyer_Records
                };

                return View(lawyer_data);


            }
            else
            {
                return RedirectToAction("loginfrom_lawyer");
            }
           
        }
        public IActionResult loginfrom_lawyer()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult loginfrom_lawyer(string lawyerEmail, string lawyerPassword)
        {
            var lawyerdetails = contexts.tbL_lawyersdetail.FirstOrDefault(law => law.lawyer_email == lawyerEmail);
             if(lawyerdetails!= null && lawyerdetails.lawyer_password==lawyerPassword)
            {
                HttpContext.Session.SetString("lawyer_session",lawyerdetails.lawyer_id.ToString());
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.errormsg = "inccorected password or email";
                return View();
            }

             

        }
        public IActionResult appoitmentDetails()
        {
            var notlogined = HttpContext.Session.GetString("lawyer_session");
            if (notlogined != null)
            {
                var Lawyer_Records = contexts.tbL_lawyersdetail.Where(e => e.lawyer_id == int.
                Parse(notlogined)).ToList();
                var appointment_booked = contexts.tbl_bokedappointment.
             Where(law => law.lawyer_id == int.Parse(notlogined)).ToList();

                lawyerViewModel lawyer_data = new lawyerViewModel()
                { lawyersdetails = Lawyer_Records,
                 get_appointments = appointment_booked
                };

                return View(lawyer_data);
            }
            else
            {
                return RedirectToAction("loginfrom_lawyer");
            }

        }

        public IActionResult delete_appoitment(int id)
        {
            var del_id = contexts.tbl_bokedappointment.Find(id);
            contexts.tbl_bokedappointment.Remove(del_id);
            contexts.SaveChanges();
            return RedirectToAction("appoitmentDetails");
        }
        public IActionResult status_appoitment(int id)
        {
             
            var findid = contexts.tbl_bokedappointment.Find(id);
            if (findid.status == 0)
            {
                findid.status = 1;
                findid.status_updates = "approved";
               

            }
            else
            {
                findid.status = 0;
                findid.status_updates = "pending";

            }
            contexts.SaveChanges();

            return RedirectToAction("appoitmentDetails");
        }
    }
}
