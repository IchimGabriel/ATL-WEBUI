using ATL_WebUI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ATL_WebUI.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
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
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole role)
        {
            try
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.Name));
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }
    }
}