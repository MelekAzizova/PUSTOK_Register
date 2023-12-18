using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.AuthVM;

namespace WebApplicationPustok.Controllers
{
	public class AuthController : Controller
	{
		public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		SignInManager<AppUser> _signInManager { get; }
		UserManager<AppUser> _userManager { get; }

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
			return View();
		}
	}
}
