using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyMaker.Models;

namespace SurveyMaker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<AppUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    [Authorize]
    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        var forms = _context.Forms
            .Where(f => f.CreatedBy == userId && f.IsRelease == true)
            // .OrderByDescending(f => f.CreatedAt)
            .ToList();

        var responses = _context.Responses
            .Where(f => f.UserId == userId)
            .Include(r => r.Form)
            // .OrderByDescending(r => r.CreatedAt)
            .ToList();

        if (forms.Count > 0)
        {
            ViewData["forms"] = forms;
        }

        if (responses.Count > 0)
        {

            ViewData["responses"] = responses;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Search(string searchString)
    {
        var userId = _userManager.GetUserId(User);

        if (!String.IsNullOrEmpty(searchString))
        {
            var forms = await _context.Forms
                .Where(f => f.Title.Contains(searchString) && f.IsRelease == true && f.CreatedBy == userId)
                .ToListAsync();

            if (forms.Count > 0)
            {
                ViewData["forms"] = forms;
            }
        }

        var responses = _context.Responses
            .Where(f => f.UserId == userId)
            .Include(r => r.Form)
            // .OrderByDescending(r => r.CreatedAt)
            .ToList();

        if (responses.Count > 0)
        {
            ViewData["responses"] = responses;
        }

        return View("Index");
    }
}
