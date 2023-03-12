using ASP_Meeting_18.Models.Domain;

namespace ASP_Meeting_18.Models.ViewModels.CartViewModels
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; } = default!;
        public string? ReturnUrl { get; set; }
    }
}
