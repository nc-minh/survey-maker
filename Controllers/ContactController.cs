using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyMaker.Data;
using SurveyMaker.Models;

namespace SurveyMaker.Controllers
{
    [Authorize(Roles = RoleName.Administrator)]
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contact
        [HttpGet("/admin/contacts")]

        public async Task<IActionResult> Index()
        {
            return _context.Contacts != null ?
                        View(await _context.Contacts.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
        }

        // GET: Contact/Details/5
        [HttpGet("/admin/contacts/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Contact/Create
        [HttpGet("/contacts")]
        [AllowAnonymous]
        public IActionResult SentContact()
        {
            // Store a value in the session
            if (HttpContext.Session.GetString("Name") == null)
            {

                HttpContext.Session.SetString("Name", "GitHub Copilot" + DateTime.Now.ToString());
            }

            var name = HttpContext.Session.GetString("Name");

            ViewData["Name"] = name;

            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/contacts/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SentContact([Bind("FullName,Email,Message,Phone")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateSent = DateTime.Now;

                _context.Add(contact);
                await _context.SaveChangesAsync();

                StatusMessage = "Cảm ơn bạn đã gửi liên hệ cho chúng tôi.";

                return RedirectToAction("Index", "Home");
            }
            return View(contact);
        }

        // GET: Contact/Delete/5
        [HttpGet("/admin/contacts/delete/{id}"), ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost("/admin/contacts/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
