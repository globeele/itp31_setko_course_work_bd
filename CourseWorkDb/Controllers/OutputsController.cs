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
    public class OutputsController : Controller
    {
        private const int PageSize = 10;
        private const string SessionKey1 = "ncr_company_filter";
        private const string SessionKey2 = "ncr_year_filter";

        private readonly RationingDbContext _context;


        public OutputsController(RationingDbContext context)
        {
            _context = context;
        }

        // GET: Outputs
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

            IQueryable<Output> list = _context.Outputs.Include(n => n.Company).Include(n => n.Product);

            if (company != null)
            {
                if (company != 0)
                {
                    HttpContext.Session.SetInt32(SessionKey1, company.Value);
                    list = list.Where(p => p.CompanyId == company);
                }
            }

            if (year != null)
            {
                if (year != 0)
                {
                    HttpContext.Session.SetInt32(SessionKey2, year.Value);
                    list = list.Where(p => p.Year == year);
                }
            }

            int count = list.Count();
            var itemList = await list.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            IndexViewModel<Output> viewModel = new IndexViewModel<Output>
            {
                PageViewModel = new PageViewModel(count, page, PageSize),
                FilterViewModel = new FilterViewModel(_context.Companies.ToList(), company, 
                    _context.Outputs.GroupBy(item => item.Year).Select(item => new FilterViewModel.Year 
                    { 
                        Name = item.Key.ToString(), 
                        Number = item.Key
                    }).OrderBy(item => item.Number).ToList(), year),
                Items = itemList
            };
            return View(viewModel);
        }

        // GET: Outputs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var output = await _context.Outputs
                .Include(n => n.Company)
                .Include(n => n.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (output == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", output.CompanyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit", output.ProductId);
            return View(output);
        }

        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit");
            return View();
        }

        // POST: Output/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OutputPlan,OutputFact,Quarter,Year,CompanyID,ProductId")] Output output)
        {
            if (_context.Outputs.FirstOrDefault(item => item.Year.Equals(output.Year)
                && item.Quarter.Equals(output.Quarter) 
                && item.CompanyId.Equals(output.CompanyId)
                && item.ProductId.Equals(output.ProductId)) != null)
            {
                ModelState.AddModelError("Year", "Запись с таким годом и кварталом для данной продукции и организации уже существует!");
                ModelState.AddModelError("Quarter", "Запись с таким годом и кварталом для данной продукции и организации уже существует!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(output);
                _context.Add(new Output
                {
                    OutputPlan = 0,
                    OutputFact = 0,
                    Quarter = output.Quarter,
                    Year = output.Year,
                    CompanyId = output.CompanyId,
                    ProductId = output.ProductId,
                    Company = output.Company,
                    Product = output.Product
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", output.CompanyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit", output.ProductId);
            return View(output);
        }

        // POST: Output/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OutputPlan,OutputFact,Quarter,Year,CompanyId,ProductId")] Output output)
        {
            if (id != output.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(output);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutputExists(output.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", output.CompanyId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "MeasureUnit", output.ProductId);
            return View(output);
        }

        // POST: Output/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var output = await _context.Outputs.FindAsync(id);
            _context.Outputs.Remove(output);
            var release = await _context.Releases.FindAsync(id);
            _context.Releases.Remove(release);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutputExists(int id)
        {
            return _context.Outputs.Any(e => e.Id == id);
        }
    }
}
