using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.ViewModels.AdminViewModels.CategoryViewModels;
using Microsoft.Extensions.Logging;
using ASP_Meeting_18.Models.DTOs.CategoryDTOs;
using AutoMapper;

namespace ASP_Meeting_18.Controllers.Admin
{
    public class CategoriesController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CategoriesController(ShopDbContext context, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<CategoriesController>();
        }

        // GET: Categories
        public async Task<IActionResult> Index(int? parentCategoryId)
        {
            IQueryable<Category> categories = _context.Categories
                .Include(c => c.ParentCategory);
            if(parentCategoryId!=null)
                categories = categories.Where(p => p.ParentCategoryId == parentCategoryId);

            IEnumerable<CategoryDTO> tempCategories = 
                _mapper.Map<IEnumerable<CategoryDTO>>(await categories.ToListAsync());

            SelectList parentCategorySL = new(await _context.Categories.ToListAsync(),
                nameof(Category.Id),
                nameof(Category.Title),
                parentCategoryId);
            IndexCategoryViewModel vm = new IndexCategoryViewModel
            {
                Categories = tempCategories,
                ParentCategorySL = parentCategorySL,
                ParentCategoryId = parentCategoryId
            };
            return View(vm);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            DetailsCategoryViewModel vm = new DetailsCategoryViewModel()
            {
                Category = _mapper.Map<CategoryDTO>(category)
            };

            return View(vm);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,ParentCategoryId")] Category category)
        public async Task<IActionResult> Create(CreateCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                SelectList parentCategoty = new(await _context.Categories.ToListAsync(),
                    nameof(Category.Id),
                    nameof(Category.Title),
                    vm.ParentCategoryId);
                foreach (var error in ModelState.Values.SelectMany(t => t.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return View(vm);
            }
            var category = new Category
            {
                Title = vm.Title,
                ParentCategoryId = vm.ParentCategoryId
            };
            Category createdCategory = _mapper.Map<Category>(category);
            _context.Add(createdCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title", category.ParentCategoryId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title", category.ParentCategoryId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var vm = new DeleteCategoryViewModel()
            {
                Category = _mapper.Map<CategoryDTO>(category) 
            };

            return View(vm);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ShopDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
