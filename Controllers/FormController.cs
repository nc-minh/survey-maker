using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyMaker.Models;

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

        [Authorize]
        [HttpGet("/forms/mode/{formId}")]
        public IActionResult Mode(int formId)
        {
            ViewData["formId"] = formId;
            return View();
        }

        [Authorize]
        [HttpGet("/forms/view/{formId}")]
        public async Task<IActionResult> View(int formId)
        {
            if (_context.Forms == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var form = await _context.Forms.FirstOrDefaultAsync(f => f.Id == formId && f.CreatedBy == userId);

            if (form == null)
            {
                return NotFound();
            }

            var questions = _context.Questions.Where(q => q.FormId == formId).ToList();


            if (questions != null && questions.Count > 0)
            {
                for (int i = 0; i < form?.Questions.Count; i++)
                {
                    if (form?.Questions[i].QuestionType == "TRAC_NGHIEM")
                    {
                        var options = _context.Options.Where(o => o.QuestionId == form.Questions[i].Id).ToList();
                        form.Questions[i].Options = options;
                    }
                }
            }

            form.Questions = questions;
            ViewData["form"] = form;
            return View();
        }

        [Authorize]
        [HttpGet("/forms/statistical/{formId}")]
        public IActionResult Statistical(int formId)
        {
            return View();
        }
    }
}