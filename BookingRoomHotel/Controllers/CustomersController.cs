using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;

namespace BookingRoomHotel.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Register(CustomerViewModel model)
        {
            if (ModelState.IsValid && model.Pw.Equals(model.PwCf))
            {
                if (_context.Customers.Find(model.Id) != null)
                {
                    TempData["Message"] = "Register Failed! ID already existed!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
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
                    TempData["Message"] = "Register Successful!";
                    return RedirectToAction("Index", "Home");
                }
                
            }
            TempData["Message"] = "Register Failed! Your password does not match!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cus = _context.Customers.Find(model.UserName);
                if (cus != null && cus.Pw.Equals(model.Password))
                {
                    TempData["Message"] = "Login Successful!";
                    HttpContext.Session.SetString("Role", "Customer");
                    HttpContext.Session.SetString("Name", cus.Name);
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Message"] = "Login Failed!";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePwViewModel model)
        {
            if (ModelState.IsValid && model.NewPw.Equals(model.ConfirmNewPw))
            {
                var cus = _context.Customers.Find(model.Id);
                if (cus != null && cus.Pw.Equals(model.OldPw))
                {
                    cus.Pw = model.NewPw;
                    _context.SaveChanges();
                    TempData["Message"] = "Change Password Successful!";
                }
                else
                {
                    TempData["Message"] = "Change Password Failed! Your information not correctly!";
                }
            }
            else
            {
                TempData["Message"] = "Change Password Failed!";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
