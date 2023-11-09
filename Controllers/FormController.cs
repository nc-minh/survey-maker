using Microsoft.AspNetCore.Mvc;
using SurveyMaker.Models;

namespace SurveyMaker.Controllers
{
    public class FormController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("/forms/create")]
        public IActionResult Create()
        {
            return View();
        }
    }
}