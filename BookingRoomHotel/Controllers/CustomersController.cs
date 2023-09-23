using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using System.Net.Mail;
using BookingRoomHotel.Models.ModelsInterface;

namespace BookingRoomHotel.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public CustomersController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return _context.Customers != null ?
                        View(await _context.Customers.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public IActionResult Register([FromForm] CusRegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.Pw.Equals(model.PwCf))
                {
                    if (_context.Customers.Find(model.Id) != null)
                    {
                        throw new Exception("ID already existed!");
                    }
                    else
                    {
                        _emailService.SendRegisterMail(model.Email, model.Name, model.Id, model.Pw);
                        var cus = new Customer
                        {
                            Id = model.Id,
                            Pw = model.Pw,
                            Name = model.Name,
                            Email = model.Email,
                            Address = model.Address,
                            DateOfBirth = model.DateOfBirth,
                            Phone = model.Phone
                        };
                        _context.Customers.Add(cus);
                        _context.SaveChanges();
                        TempData["Success"] = "Register Successful! Please check your email!";
                        return Json(new {success = true});
                    }

                }
                throw new Exception("Your password does not match!");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Register Failed! Error: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Login([FromForm] CusLoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cus = _context.Customers.Find(model.UserName);
                    if (cus != null && cus.Pw.Equals(model.Password))
                    {
                        HttpContext.Session.SetString("Role", "Customer");
                        HttpContext.Session.SetString("Name", cus.Name);
                        TempData["Success"] = "Login Successful!";
                        return Json(new {success = true});
                    }
                    else
                    {
                        throw new Exception("ID or Password not correct!");
                    }
                }
                else
                {
                    throw new Exception("Your input not correct!");
                }
            }
            catch (Exception ex)
            {
                return Json(new {success = false, error = "Login Failed! Error: " + ex.Message});
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ChangePassword([FromForm] CusChangePwViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.NewPw.Equals(model.ConfirmNewPw))
                {
                    var cus = _context.Customers.Find(model.Id);
                    if (cus != null && cus.Pw.Equals(model.OldPw))
                    {

                        cus.Pw = model.NewPw;
                        _context.SaveChanges();
                        _emailService.SendChangePasswordMail(cus.Email, cus.Name, cus.Pw);
                        TempData["Success"] = "Change Password successful! Please check your email!";
                        return Json(new { success = true });
                    }
                    else
                    {
                        throw new Exception("Your information not correctly!");
                    }
                }
                else
                {
                    throw new Exception("Your input not correct!");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Change Password Failed! Error: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ForgotPassword([FromForm] CusForgotPasswordViewModel model)
        {
            try
            {
                var cus = _context.Customers.Find(model.Id);
                if (cus != null && cus.Email.Equals(model.Email))
                {
                    _emailService.SendForgotPasswordMail(cus.Email, cus.Name, cus.Pw);
                    TempData["Success"] = "Your password has been sent via email. Please check your email!";
                    return Json(new { success = true });
                }else 
                { 
                    throw new Exception("Your ID or Email not correct!"); 
                }
            }catch (Exception ex)
            {
                return Json(new { success = false, error = "Get password Failed! Error: " + ex.Message });
            }
        }

    }

}
