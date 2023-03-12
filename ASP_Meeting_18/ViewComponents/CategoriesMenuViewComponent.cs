using ASP_Meeting_18.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Meeting_18.ViewComponents
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly ShopDbContext context;

        public CategoriesMenuViewComponent(ShopDbContext context) 
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? currentCategory)
        {           
            List<string> categoryNames = await context.Products.Include(t=>t.Category)
                .Select(t=>t.Category!.Title)
                .Distinct().ToListAsync();
            return View(new Tuple<List<string>, string?>(categoryNames, currentCategory));
        }
    }
}
