using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseWorkDb.Models;
using CourseWorkDb.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using CourseWorkDb.ViewModels;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace CourseWorkDb.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private const int PageSize = 10;
        private const string SessionKey1 = "viol_company_filter";
        private const string SessionKey2 = "viol_year_filter";
        private const string SessionKey3 = "cmpr_company_filter";
        private const string SessionKey4 = "cmpr_year_filter";

        private readonly RationingDbContext _context;

        public CompaniesController(RationingDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index(string searchByName, int page = 1)
        {
            ViewData["CurrentSearch"] = searchByName;

            IQueryable<Company> source = _context.Companies;

            if (!string.IsNullOrEmpty(searchByName))
            {
                source = source.Where(item => item.CompanyName.Contains(searchByName));
            }

            List<Company> list = await source.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            IndexViewModel<Company> viewModel = new IndexViewModel<Company>
            {
                PageViewModel = new PageViewModel(await source.CountAsync(), page, PageSize),
                Items = list
            };
            return View(viewModel);
        }

        // GET: Companys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyName,FormOfOwnership,HeadName,ActivityType")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,FormOfOwnership,HeadName,ActivityType")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }

        // GET: Companies/Violators
        public async Task<IActionResult> Violators(int? company, int? year, int page = 1)
        {
            if (company == null)
            {
                company = HttpContext.Session.GetInt32(SessionKey1);
            }

            if (year == null)
            {
                year = HttpContext.Session.GetInt32(SessionKey2);
            }

            var list = _context.Outputs.Include(item => item.Company).Include(item => item.Product)
                .Join(_context.Releases, a => new { a.Year, a.Quarter, a.CompanyId, a.ProductId }, b => new { b.Year, b.Quarter, b.CompanyId, b.ProductId },
                (a, b) => new 
                {
                    a.CompanyId,
                    a.Company.CompanyName,
                    a.Year,
                    a.Quarter,
                    a.Product.ProductName,
                    a.Product.MeasureUnit,
                    a.Product.ProductType,
                    Excess = (b.ReleaseFact / b.ReleasePlan) - a.OutputFact
                }).Where(item => item.Excess > 0.0f);

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

            List<ComparisonViewModel> itemList = await list.Skip((page - 1) * PageSize).Take(PageSize)
                .Select(item => new ComparisonViewModel
                {
                    CompanyId = item.CompanyId,
                    CompanyName = item.CompanyName,
                    ProductName = item.ProductName,
                    MeasureUnit = item.MeasureUnit,
                    Year = item.Year,
                    Quarter = item.Quarter,
                    Excess = item.Excess
                }).ToListAsync();

            IndexViewModel<ComparisonViewModel> viewModel = new IndexViewModel<ComparisonViewModel>
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

        // GET: Companies/Comparison
        public async Task<IActionResult> Comparison(int? company, int? year, int page = 1)
        {
            if (company == null)
            {
                company = HttpContext.Session.GetInt32(SessionKey3);
            }

            if (year == null)
            {
                year = HttpContext.Session.GetInt32(SessionKey4);
            }

            var list = _context.Releases.Include(item => item.Company).Include(item => item.Product)
                .Join(_context.Outputs, a => new { a.Year, a.Quarter, a.CompanyId, a.ProductId }, b => new { b.Year, b.Quarter, b.CompanyId, b.ProductId },
                (a, b) => new
                {
                    a.CompanyId,
                    a.Company.CompanyName,
                    a.Product.ProductName,
                    a.Product.ProductType,
                    a.Product.MeasureUnit,
                    a.Year,
                    a.Quarter,
                    a.ReleasePlan,
                    a.ReleaseFact,
                    OutputDifference = b.OutputFact / b.OutputPlan
                });

            if (company != null)
            {
                HttpContext.Session.SetInt32(SessionKey3, company.Value);
                if (company != 0)
                {
                    list = list.Where(p => p.CompanyId == company);
                }
            }

            if (year != null)
            {
                HttpContext.Session.SetInt32(SessionKey4, year.Value);
                if (year != 0)
                {
                    list = list.Where(p => p.Year == year);
                }
            }

            int count = list.Count();

            List<ComparisonViewModel> itemList = await list.Skip((page - 1) * PageSize).Take(PageSize)
                .Select(item => new ComparisonViewModel
                {
                    CompanyId = item.CompanyId,
                    CompanyName = item.CompanyName,
                    MeasureUnit = item.MeasureUnit,
                    ProductName = item.ProductName,
                    Year = item.Year,  
                    Quarter = item.Quarter,
                    ReleaseFact = item.ReleaseFact,
                    ReleasePlan = item.ReleasePlan
                }).ToListAsync();

            IndexViewModel<ComparisonViewModel> viewModel = new IndexViewModel<ComparisonViewModel>
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
    }
}
