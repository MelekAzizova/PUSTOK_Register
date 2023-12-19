using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.AuthVM;

namespace WebApplicationPustok.Controllers
{
	public class AuthController : Controller
	{
		public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		SignInManager<AppUser> _signInManager { get; }
		UserManager<AppUser> _userManager { get; }
		RoleManager<IdentityRole> _roleManager { get; }

		//==============================Registr==========================
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			var result = await _userManager.CreateAsync(new AppUser
			{
				Fullname = vm.Fullname,
				Email = vm.Email,
				UserName = vm.Username
			}, vm.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(vm);
			}
			return RedirectToAction("Index", "Home");
		}

		//=====================================Login============================
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginVM vm)
		{
			AppUser user;
			if (vm.UsernameOrEmail.Contains("@"))
			{
				user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
			}
			else
			{
				user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
			}
			var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
			if (!result.Succeeded)
			{
				if (result.IsLockedOut)
				{
					ModelState.AddModelError("", "Too many attempts wait until " + DateTime.Parse(user.LockoutEnd.ToString()).ToString("HH:mm"));
				}
				else
				{
					ModelState.AddModelError("", "Username or password is wrong");
				}
				return View(vm);
			}
			//if (returnUrl != null)
			//{
			//	return LocalRedirect(returnUrl);
			//}
			return RedirectToAction("Index", "Home");
			
		}
	}
}
