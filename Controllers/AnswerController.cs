using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyMaker.Models;

namespace SurveyMaker.Controllers;

public class AnswerController : Controller
{
    private readonly ILogger<AnswerController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AnswerController(ApplicationDbContext context, ILogger<AnswerController> logger, UserManager<AppUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("/answers/upsert")]
    public async Task<IActionResult> UpsertAnswer(int questionId, int responseId, string? content, int? optionId)
    {
        _logger.LogInformation("UpsertAnswer: " + questionId + " " + responseId + " " + content + " " + optionId);
        var response = await _context.Responses.FindAsync(responseId);
        var question = await _context.Questions.FindAsync(questionId);
        var option = await _context.Options.FindAsync(optionId);

        if (response == null || question == null)
        {
            return NotFound();
        }

        var oldAnswer = await _context.Answers.Where(a => a.QuestionId == questionId && a.ResponseId == responseId).FirstOrDefaultAsync();
        _logger.LogInformation("OldAnswer: " + oldAnswer);
        if (oldAnswer != null)
        {

            if (question.QuestionType == "TRAC_NGHIEM" && optionId != null && option != null && option.QuestionId == questionId)
            {
                oldAnswer.OptionId = optionId;
            }

            oldAnswer.content = content;

            oldAnswer.UpdatedAt = DateTime.Now;
            _context.Answers.Update(oldAnswer);
            await _context.SaveChangesAsync();

            return Ok(oldAnswer);
        }



        var newAnswer = new AnswerModel
        {
            QuestionId = questionId,
            ResponseId = responseId,
            content = content,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        if (question.QuestionType == "TRAC_NGHIEM" && optionId != null && option != null && option.QuestionId == questionId)
        {
            newAnswer.OptionId = optionId;
        }

        _context.Answers.Add(newAnswer);
        await _context.SaveChangesAsync();

        return Ok(newAnswer);
    }

}
