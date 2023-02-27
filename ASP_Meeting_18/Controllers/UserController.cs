using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Meeting_18.Controllers
{
    //[Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager) 
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = userManager.Users;
            //use AutoMapper in Future!!!
            IEnumerable<UserDTO> userDTO = await users.Select(t=>new UserDTO 
            { 
                Id = t.Id, 
                Login = t.UserName,
                Email = t.Email, 
                YearOfBirth = t.YearOfBirth 
            }).ToListAsync();
            return View(userDTO);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = dto.Email,
                    UserName = dto.Login,
                    YearOfBirth = dto.YearOfBirth
                };
                var result = await userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
                return NotFound();
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            //AutoMapper!!!
            EditUserDTO dto = new EditUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.UserName,
                YearOfBirth = user.YearOfBirth
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(dto.Id);
                if (user == null)
                {
                    return NotFound();
                }
                user.UserName = dto.Login;
                user.YearOfBirth = dto.YearOfBirth;
                user.Email = dto.Email;
                IdentityResult result = await userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> ChangePassword(string? id)
        {
            if(id == null) return NotFound();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordDTO dto = new ChangePasswordDTO()
            {
                Id = user.Id,
                Email = user.Email,

            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync (dto.Id);
                var passwordValidator = HttpContext.RequestServices.GetRequiredService<IPasswordValidator<User>>();
                var passwordHasher = HttpContext.RequestServices.GetRequiredService<IPasswordHasher<User>>();

                if (user == null) return NotFound();
                
                var identityResult = await passwordValidator.ValidateAsync(userManager, user, dto.NewPassword);
                if (identityResult.Succeeded)
                {
                    string hashedPassword = passwordHasher.HashPassword(user, dto.NewPassword);
                    user.PasswordHash = hashedPassword;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(dto);
        }
    }
}