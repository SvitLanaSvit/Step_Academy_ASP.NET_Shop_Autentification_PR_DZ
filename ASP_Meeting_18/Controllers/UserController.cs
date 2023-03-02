using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTOs.UserDTOs;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IMapper mapper) 
        {
            this.userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var users = userManager.Users;
            //use AutoMapper in Future!!!
            //IEnumerable<UserDTO> userDTO = await users.Select(t=>new UserDTO 
            //{ 
            //    Id = t.Id, 
            //    Login = t.UserName,
            //    Email = t.Email, 
            //    YearOfBirth = t.YearOfBirth 
            //}).ToListAsync();
            IEnumerable<UserDTO> userDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            return View(userDTO);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                //User user = new User
                //{
                //    Email = dto.Email,
                //    UserName = dto.Login,
                //    YearOfBirth = dto.YearOfBirth
                //};
                User user = _mapper.Map<User>(dto);
                var result = await userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index");
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
            //EditUserDTO dto = new EditUserDTO
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    Login = user.UserName,
            //    YearOfBirth = user.YearOfBirth
            //};
            EditUserDTO dto = _mapper.Map<EditUserDTO>(user);
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
            //ChangePasswordDTO dto = new ChangePasswordDTO()
            //{
            //    Id = user.Id,
            //    Email = user.Email,

            //};
            ChangePasswordDTO dto = _mapper.Map<ChangePasswordDTO>(user);
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

        public async Task<IActionResult> Delete(string? id)
        {
            if(id == null) return NotFound();
            var user = await userManager.FindByIdAsync (id);
            if (user == null) return NotFound();
            //DeleteUserDTO userDTO = new DeleteUserDTO
            //{
            //    Id = user.Id,
            //    Login = user.UserName,
            //    Email = user.Email,
            //    YearOfBirth = user.YearOfBirth

            //};
            DeleteUserDTO userDTO = _mapper.Map<DeleteUserDTO>(user);
            return View(userDTO);
        }

        [HttpPost,  ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(userManager.Users == null) return NotFound();

            User user = await userManager.FindByIdAsync(id); 
            if (user == null) return NotFound();

            //IdentityResult result =
            await userManager.DeleteAsync(user);
            //if(result.Succeeded)
            //{
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    return View((object)id);
            //}
            return RedirectToAction("Index");
        }
    }
}