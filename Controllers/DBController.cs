using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurveyMaker.Data;
using SurveyMaker.Models;

namespace SurveyMaker.Controllers;

public class DBController : Controller
{
    private readonly ILogger<DBController> _logger;
    private readonly UserManager<AppUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    public DBController(ILogger<DBController> logger, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
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

    [TempData]
    public string StatusMessage { get; set; }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> SeedDataAsync()
    {
        var rolenames = typeof(RoleName).GetFields().ToList();
        foreach (var role in rolenames)
        {
            var rolename = (string?)role.GetRawConstantValue();
            if (rolename != null)
            {
                var rfound = await _roleManager.FindByNameAsync(rolename);
                if (rfound == null)
                {
                    var r = new IdentityRole(rolename);
                    await _roleManager.CreateAsync(r);
                }
            }
        }

        // Create admin, username=admin, password=Admin123!, role=Administrator, mail=admin@localhost
        var useradmin = await _userManager.FindByNameAsync("admin");
        if (useradmin == null)
        {
            var user = new AppUser()
            {
                UserName = "admin",
                Email = "admin@localhost",
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, "Admin123!");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleName.Administrator);
            }
        }

        StatusMessage = "Vừa thực hiện seed dữ liệu thành công!";
        return RedirectToAction("Index");
    }
}
