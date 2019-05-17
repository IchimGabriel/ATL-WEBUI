using ATL_WebUI.Data;
using ATL_WebUI.Models.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Controllers
{
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

        // GET: Account
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            var roles = _context.Roles.ToList();
            ViewBag.Name = new SelectList(roles, "Name", "Name");
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser { Id = Guid.NewGuid().ToString(), UserName = model.UserName, Email = model.Email, PhoneNumber = model.Phone };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var role = await _userManager.FindByEmailAsync(model.Email);
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