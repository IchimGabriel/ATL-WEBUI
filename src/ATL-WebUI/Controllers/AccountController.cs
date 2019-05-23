using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin, Broker")]
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var roles = _context.Roles.ToList();
            ViewBag.Name = new SelectList(roles, "Name", "Name");
            return View();
        }

        [Authorize(Roles = "Broker")]
        public ActionResult CreateCustomer()
        {
            var role = _context.Roles.FirstOrDefault(c => c.Name == "Customer");
            ViewBag.Name = role;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Broker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser { Id = Guid.NewGuid().ToString(), UserName = model.UserName, Email = model.UserName, PhoneNumber = model.Phone };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var role = await _userManager.FindByEmailAsync(model.UserName);
                        if (!await _userManager.IsInRoleAsync(role, model.UserRoles))
                        {
                            await _userManager.AddToRoleAsync(role, model.UserRoles);
                            await _userManager.UpdateAsync(role);
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(model);
            }
            catch
            {
                return View();
            }
        }
    }
}