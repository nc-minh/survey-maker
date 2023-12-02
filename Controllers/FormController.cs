using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SurveyMaker.Models;
using Microsoft.EntityFrameworkCore;

namespace SurveyMaker.Controllers
{
    public class FormController : Controller
    {
        private readonly ILogger<FormController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public FormController(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<FormController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("/forms/create")]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var oldForm = _context.Forms.Where(f => f.CreatedBy == userId).Where(f => f.IsRelease == false).ToList();

            if (oldForm.Count > 0)
            {
                _logger.LogInformation("User has already created a form");

                var questions = _context.Questions.Where(q => q.FormId == oldForm[0].Id).ToList();

                for (int i = 0; i < questions.Count; i++)
                {
                    if (questions[i].QuestionType == "TRAC_NGHIEM")
                    {
                        var options = _context.Options.Where(o => o.QuestionId == questions[i].Id).ToList();
                        questions[i].Options = options;
                    }
                }

                oldForm[0].Questions = questions;

                ViewData["data"] = oldForm[0];
                return View();
            }

            var newForm = new FormModel
            {
                CreatedBy = userId,
                IsRelease = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Add(newForm);
            await _context.SaveChangesAsync();

            ViewData["data"] = newForm;
            return View();
        }

        [Authorize]
        [HttpPost("/forms/save")]
        public async Task<IActionResult> SaveForm(string title, string description)
        {
            var userId = _userManager.GetUserId(User);
            var oldForm = _context.Forms.Where(f => f.CreatedBy == userId).Where(f => f.IsRelease == false).ToList();
            if (oldForm == null)
            {
                return NotFound();
            }

            oldForm[0].Title = title;
            oldForm[0].Description = description;
            oldForm[0].UpdatedAt = DateTime.Now;
            oldForm[0].IsRelease = true;

            _context.Forms.Update(oldForm[0]);
            await _context.SaveChangesAsync();

            return Ok(oldForm);
        }
    }
}