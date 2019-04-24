using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATL_WebUI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATL_WebUI.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        // GET: Role
        public ActionResult Index()
        {
            var roles = _context.Roles.ToList();
            if (roles != null)
            {
                return View(roles);
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View(role);
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create(IdentityRole role)
        {
            try
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}