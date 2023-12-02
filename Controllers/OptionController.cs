using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurveyMaker.Models;
using System.Data.Entity;

namespace SurveyMaker.Controllers
{
    public class OptionController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OptionController> _logger;

        public OptionController(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<OptionController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("/options/create")]
        public async Task<IActionResult> Create(int questionId, string[] content)
        {
            _logger.LogInformation("Create option");
            _logger.LogInformation("Question id: " + questionId);
            _logger.LogInformation("Content: " + content);

            var question = await _context.Questions.FindAsync(questionId);

            if (question == null)
            {
                return NotFound();
            }

            foreach (var item in content)
            {
                var option = new OptionModel
                {
                    Content = item,
                    QuestionId = questionId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Options.Add(option);

            }


            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("/options/create/one")]
        public async Task<IActionResult> CreateOne(int questionId, string content)
        {
            var question = await _context.Questions.FindAsync(questionId);

            if (question == null)
            {
                return NotFound();
            }

            var option = new OptionModel
            {
                Content = content,
                QuestionId = questionId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Options.Add(option);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [Authorize]
        [HttpPost("/options/update")]
        public async Task<IActionResult> Update(int id, string content)
        {
            var option = await _context.Options.FindAsync(id);

            if (option == null)
            {
                return NotFound();
            }

            option.Content = content;
            option.UpdatedAt = DateTime.Now;

            _context.Options.Update(option);
            await _context.SaveChangesAsync();

            return Ok(option);
        }

        [Authorize]
        [HttpPost("/options/delete")]
        public async Task<IActionResult> DeleteOption(int optionId)
        {

            _logger.LogInformation("Delete option {optionId}", optionId);

            var oldOption = await _context.Options.FindAsync(optionId);
            if (oldOption != null)
            {
                _context.Options.Remove(oldOption);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}