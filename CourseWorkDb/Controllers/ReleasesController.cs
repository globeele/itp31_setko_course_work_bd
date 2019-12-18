using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWorkDb.Models;
using CourseWorkDb.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using CourseWorkDb.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CourseWorkDb.Controllers
{
    [Authorize]
    public class ReleasesController : Controller
    {
        private const int PageSize = 10;
        private const string SessionKey1 = "pad_company_filter";
        private const string SessionKey2 = "pad_year_filter";

        private readonly RationingDbContext _context;

        public ReleasesController(RationingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? company, int? year, int page = 1)
        {
            if (company == null)
            {
                company = HttpContext.Session.GetInt32(SessionKey1);
            }

            if (year == null)
            {
                year = HttpContext.Session.GetInt32(SessionKey2);
            }

            IQueryable<Release> list = _context.Releases.Include(p => p.Company).Include(p => p.Product);

            if (company != null)
            {
                HttpContext.Session.SetInt32(SessionKey1, company.Value);
                if (company != 0)
                {
                    list = list.Where(p => p.CompanyId == company);
                }
            }

            if (year != null)
            {
                HttpContext.Session.SetInt32(SessionKey2, year.Value);
                if (year != 0)
                {
                    list = list.Where(p => p.Year == year);
                }
            }

            int count = list.Count();
            var itemList = await list.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            IndexViewModel<Release> viewModel = new IndexViewModel<Release>
            {
                PageViewModel = new PageViewModel(count, page, PageSize),
                FilterViewModel = new FilterViewModel(_context.Companies.ToList(), company,
                    _context.Releases.GroupBy(item => item.Year).Select(item => new FilterViewModel.Year
                    {
                        Name = item.Key.ToString(),
                        Number = item.Key
                    }).OrderBy(item => item.Number).ToList(), year),
                Items = itemList
            };
            return View(viewModel);
        }

        // GET: Releases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var release = await _context.Releases
                .Include(p => p.Company)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (release == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", release.CompanyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit", release.ProductId);
            return View(release);
        }

        // GET: Releases/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReleasePlan,ReleaseFact,Quarter,Year,CompanyId,ProductId")] Release release)
        {
            if (_context.Releases.FirstOrDefault(item => item.Year.Equals(release.Year)
                && item.Quarter.Equals(release.Quarter) 
                && item.CompanyId.Equals(release.CompanyId)
                && item.ProductId.Equals(release.ProductId)) != null)
            {
                ModelState.AddModelError("Year", "Запись с таким годом и кварталом для данной продукции и организации уже существует!");
                ModelState.AddModelError("Quarter", "Запись с таким годом и кварталом для данной продукции и организации уже существует!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(release);
                _context.Add(new Release
                {
                    ReleasePlan = 0,
                    ReleaseFact = 0,
                    Quarter = release.Quarter,
                    Year = release.Year,
                    CompanyId = release.CompanyId,
                    ProductId = release.ProductId,
                    Company = release.Company,
                    Product = release.Product
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", release.CompanyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit", release.ProductId);
            return View(release);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReleasePlan,ReleaseFact,Quarter,Year,CompanyId,ProductId")] Release release)
        {
            if (id != release.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(release);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReleaseExists(release.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", release.CompanyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit", release.ProductId);
            return View(release);
        }

        // POST: Releases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var release = await _context.Releases.FindAsync(id);
            _context.Releases.Remove(release);
            var output = await _context.Outputs.FindAsync(id);
            _context.Outputs.Remove(output);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReleaseExists(int id)
        {
            return _context.Releases.Any(e => e.Id == id);
        }
    }
}
