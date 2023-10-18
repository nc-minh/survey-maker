using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SurveyMaker.Models;

namespace SurveyMaker.Controllers;

public class DBController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public DBController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("/db-info")]
    public IActionResult Index()
    {
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
}
