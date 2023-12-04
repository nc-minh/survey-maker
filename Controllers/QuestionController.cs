using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SurveyMaker.Models;
using System.Data.Entity;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SurveyMaker.Controllers
{
    public class QuestionController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<QuestionController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("/questions/create")]
        public async Task<IActionResult> Create(int formId)
        {

            var questionIsNotDone = _context.Questions.Where(q => q.FormId == formId).Where(q => q.IsDone == false);
            _logger.LogInformation("Check question is not done {questionIsNotDone}", questionIsNotDone);

            if (questionIsNotDone.Count() > 0)
            {
                _logger.LogInformation("Question is not done", questionIsNotDone.First());

                return Ok(questionIsNotDone.First());
            }

            // var questionIsNotDone = (from f in _context.Forms select f).Where(f => f.Id == formId).Where(f => f.IsRelease == false).Include(f => f.Questions).Where(f => f.Questions.Any(q => q.IsDone == false)).ToList();

            if (questionIsNotDone.Count() > 0)
            {
                _logger.LogInformation("Question is not done", questionIsNotDone.First());

                return Ok(questionIsNotDone.First());
            }

            _logger.LogInformation("Create question {formId}", formId);

            var newQuestion = new QuestionModel
            {
                FormId = formId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Questions.Add(newQuestion);
            await _context.SaveChangesAsync();

            return Ok(newQuestion);
        }


        [Authorize]
        [HttpPost("/questions/update")]
        public async Task<IActionResult> Update(int questionId, string questionType, string questionTitle, IFormFile imageFile)

        {

            _logger.LogInformation("Update question {questionId}", questionId);

            var oldQuestion = await _context.Questions.FindAsync(questionId);

            if (oldQuestion == null)
            {
                return NotFound();
            }

            // update image
            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "StaticFiles", imageFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                oldQuestion.ImageUrl = "/StaticFiles/" + imageFile.FileName;
            }



            oldQuestion.UpdatedAt = DateTime.Now;
            oldQuestion.QuestionType = questionType;
            oldQuestion.QuestionTitle = questionTitle;
            oldQuestion.IsDone = true;

            _context.Questions.Update(oldQuestion);
            await _context.SaveChangesAsync();

            return Ok(oldQuestion);
        }

        [Authorize]
        [HttpPost("/questions/delete")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {

            _logger.LogInformation("Delete question {questionId}", questionId);

            var oldQuestion = await _context.Questions.FindAsync(questionId);
            if (oldQuestion != null)
            {
                _context.Questions.Remove(oldQuestion);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("/questions/{questionId}")]
        public async Task<IActionResult> GetQuestionById(int questionId)
        {
            _logger.LogInformation("Get question by id {questionId}", questionId);

            var question = await _context.Questions.FindAsync(questionId);

            if (question == null)
            {
                return NotFound();
            }

            var option = _context.Options.FromSqlRaw("SELECT * FROM Options WHERE QuestionId = {0}", questionId).ToList();

            question.Options = option;

            return Ok(question);
        }

        // GET: /questions/edit/{id}
        [Authorize]
        [HttpGet("/questions/edit/{id}")]
        public async Task<IActionResult> EditQuestion(int? id)
        {
            _logger.LogInformation("Get question by id {questionId}", id);

            var question = await _context.Questions.FindAsync(id);

            if (question == null || id == null)
            {
                return NotFound();
            }

            var option = _context.Options.FromSqlRaw("SELECT * FROM Options WHERE QuestionId = {0}", id).ToList();

            question.Options = option;

            ViewData["question"] = question;
            return View();
        }
    }
}