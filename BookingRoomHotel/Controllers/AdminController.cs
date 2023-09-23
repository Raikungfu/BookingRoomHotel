using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoomHotel.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context){ 
            _context = context;
        }
        public IActionResult Index()
        {
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View();
        }

        public IActionResult Login(StaffLoginViewModel model)
        {
            var staff = _context.Staffs.Find(model.Username);
            if (ModelState.IsValid && staff != null)
            {
                if (staff.Pw.Equals(model.Password))
                {
                    TempData["Message"] = "Login Successful!";
                    return RedirectToAction("Dashboard");
                }
            }
            TempData["Message"] = "Login Failed!";
            return RedirectToAction("Index","Admin");
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
