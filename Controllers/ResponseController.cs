using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyMaker.Models;

namespace SurveyMaker.Controllers;

public class ResponseController : Controller
{
    private readonly ILogger<ResponseController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;


    [TempData]
    public string StatusMessage { get; set; }

    public ResponseController(ApplicationDbContext context, ILogger<ResponseController> logger, UserManager<AppUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("/take/{formId}")]
    public async Task<IActionResult> TakeSurvey(int formId)
    {
        var userId = _userManager.GetUserId(User);
        var form = await _context.Forms.FindAsync(formId);

        if (form == null || form.IsRelease == false)
        {
            return NotFound();
        }

        var response = await _context.Responses.Where(r => r.FormId == formId && r.UserId == userId).FirstOrDefaultAsync();

        _logger.LogInformation("User response.Count: " + response);

        if (response?.IsComplete == true)
        {
            StatusMessage = "Bạn đã thực hiện khảo sát này rồi!";
            return RedirectToAction("Index", "Home");
        }

        if (response == null)
        {
            var newResponse = new ResponseModel
            {
                FormId = formId,
                UserId = userId!,
                IsComplete = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Responses.Add(newResponse);
            await _context.SaveChangesAsync();
        }

        var reGetResponse = _context.Responses.Where(r => r.FormId == formId && r.UserId == userId).ToList();
        var questions = _context.Questions.Where(q => q.FormId == form.Id).ToList();
        for (int i = 0; i < questions.Count; i++)
        {
            if (questions[i].QuestionType == "TRAC_NGHIEM")
            {
                var options = _context.Options.Where(o => o.QuestionId == questions[i].Id).ToList();
                questions[i].Options = options;
            }

            if (reGetResponse != null && reGetResponse.Count > 0)
            {
                var answers = _context.Answers.Where(a => a.ResponseId == reGetResponse[0].Id).Where(a => a.QuestionId == questions[i].Id).ToList();

                if (answers != null)
                {
                    questions[i].Answers = answers;
                }
            }

        }
        form.Questions = questions;
        form.Responses = reGetResponse;

        ViewData["form"] = form;
        return View();
    }

    [Authorize]
    [HttpPost("/responses/submit")]
    public async Task<IActionResult> SubmitSurvey(int responseId)
    {
        var response = _context.Responses.Find(responseId);
        if (response == null)
        {
            return NotFound();
        }

        response.IsComplete = true;
        response.UpdatedAt = DateTime.Now;
        _context.Responses.Update(response);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}
