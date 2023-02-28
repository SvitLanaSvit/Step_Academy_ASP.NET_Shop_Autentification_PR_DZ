using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_Meeting_18.Data;
using Microsoft.Extensions.Hosting;
using ASP_Meeting_18.Models.ViewModels.AdminViewModels.ProductViewModels;
using System.Drawing.Drawing2D;

namespace ASP_Meeting_18.Controllers.Admin
{
    public class ProductsController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(ShopDbContext context, ILoggerFactory loggerFactory, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ProductsController>();
            _environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? categoryId)
        {
            //var shopDbContext = _context.Products.Include(p => p.Category).Include(t=>t.Photos);
            //return View(await shopDbContext.ToListAsync());

            IQueryable<Product> products = _context.Products
                .Include(p => p.Category)
                .Include(t => t.Photos);
            IQueryable<Category> categories = _context.Categories;
            SelectList categorySL = new SelectList(
                await categories.ToListAsync(),
                dataValueField: nameof(Category.Id),
                dataTextField: nameof(Category.Title),
                selectedValue: categoryId);
            IndexProductViewModel vm = new IndexProductViewModel
            {
                Products = products,
                CategorySL = categorySL,
                CategoryId = categoryId
            };
            return View(vm);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(t => t.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            DetailsProductViewModel vm = new DetailsProductViewModel
            {
                Product = product
            };

            return View(vm);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Price,Count,CategoryId")] Product product,
        //    IEnumerable<IFormFile> filePaths)
        public async Task<IActionResult> Create(CreateProductViewModel vm, IEnumerable<IFormFile> filePaths)
        {
            if (!ModelState.IsValid)
            {
                SelectList categorySL = new (await _context.Categories.ToListAsync(),
                    nameof(Category.Id),
                    nameof(Category.Title),
                    vm.Product.CategoryId);
                vm.CategorySL = categorySL;
                foreach (var error in ModelState.Values.SelectMany(t => t.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return View(vm);
            }

            _context.Add(vm.Product);
            await _context.SaveChangesAsync();

            foreach (var filePath in vm.Photos!)
            {
                if (filePath != null && filePath.Length > 0)
                {
                    string filename = $"/images/{filePath.FileName}";
                    string fileFullpath = _environment.WebRootPath + filename;

                    using (var stream = new FileStream(fileFullpath, FileMode.Create, FileAccess.Write))
                    {
                        filePath.CopyTo(stream);
                    }

                    var photo = new Photo
                    {
                        Filename = filePath.FileName,
                        PhotoUrl = filename,
                        ProductId = vm.Product.Id
                    };

                    _context.Photos.Add(photo);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Count,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(f => f.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            DeleteProductModelView vm = new DeleteProductModelView
            {
                Product = product
            };

            return View(vm);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShopDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
