using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASP_Meeting_18.Models.DTOs.UserDTOs;

namespace ASP_Meeting_18.Data
{
    public class ShopDbContext : IdentityDbContext<User>
    {
        public DbSet<Photo> Photos { get; set; } = default!;

        public DbSet<Category> Categories { get; set; } = default!;

        public DbSet<Product> Products { get; set; } = default!;

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ASP_Meeting_18.Models.DTOs.UserDTOs.UserDTO> UserDTO { get; set; }

        public DbSet<ASP_Meeting_18.Models.DTOs.UserDTOs.DeleteUserDTO> DeleteUserDTO { get; set; }

        //public DbSet<ASP_Meeting_18.Models.DTOs.UserDTOs.EditUserDTO> EditUserDTO { get; set; }

        //public DbSet<ASP_Meeting_18.Models.DTOs.UserDTOs.ChangePasswordDTO> ChangePasswordDTO { get; set; }
    }
}