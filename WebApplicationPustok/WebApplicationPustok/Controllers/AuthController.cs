﻿using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.Security.Policy;
using WebApplicationPustok.ExternalService.Interfaces;
using WebApplicationPustok.Helpers;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.AuthVM;

namespace WebApplicationPustok.Controllers
{
	public class AuthController : Controller
	{
		public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_emailService = emailService;
		}

		SignInManager<AppUser> _signInManager { get; }
		UserManager<AppUser> _userManager { get; }
		RoleManager<IdentityRole> _roleManager { get; }
		IEmailService _emailService { get; }
		

		





        #region Registr
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
			var user = new AppUser
			{
				Fullname = vm.Fullname,
				Email = vm.Email,
				UserName = vm.Username
			};
			var result = await _userManager.CreateAsync(user, vm.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(vm);
			}
			var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
			if (!roleResult.Succeeded)
			{
				ModelState.AddModelError("", "Something went wrong. Please contact admin");
				return View(vm);
			}

			await _sendConfirmation(user);
			return View();
		}

        #endregion

		public IActionResult SendMail()
		{
			_emailService.Send("mi7ehrg0p@code.edu.az", "Azizova", "Yemin ederim bezdim");
			return Ok();
		}




        
        #region Email
        public async	Task<IActionResult> SendConfrimationEmail(string username)
		{
			await _sendConfirmation(await _userManager.FindByNameAsync(username));
			return Content("Email sent");

        }
		async Task _sendConfirmation(AppUser user)
		{
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var link = Url.Action("EmailConfirm", "Auth", new
			{
				token = token,
				username = user.UserName
			}, Request.Scheme);
			using StreamReader reader= new StreamReader(Path.Combine(PathConstants.RootPath, "templateEmailHtml.html"));
			string temp=reader.ReadToEnd();
			temp=temp.Replace("[[[username]]]", user.UserName);
			temp = temp.Replace("[[[link]]]", link); 
            _emailService.Send(user.Email,"Email confrim", temp);
			
        }
		public async Task<IActionResult> EmailConfirm(string token,string username)
		{
			var result=await _userManager.ConfirmEmailAsync(await _userManager.FindByNameAsync(username),token);
			if (result.Succeeded) return Ok();
			return Problem();
		}





        #endregion





        #region Login  
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
				else if (!user.EmailConfirmed)
				{
					var param = new
					{
						username = user.UserName
					};
					ViewBag.Link = Url.Action("SendConfrimationEmail", param);
					ModelState.AddModelError("", "");
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
			// }
				return RedirectToAction("Index", "Home");
			
		}
#endregion


        #region Logout
        public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
        #endregion


	    #region Roles
        public async Task<bool> CreatedRoles()
		{
			foreach (var item in Enum.GetValues(typeof(Roles)))
			{
				if (!await _roleManager.RoleExistsAsync(item.ToString()))
				{
					var result = await _roleManager.CreateAsync(new IdentityRole
					{
						Name = item.ToString()
					});
					if (!result.Succeeded)
					{
						return false;
					}
				}
			}
			return true;
		}
        #endregion



        #region Userpage(tam deyil)

        //===============================Userpage===================================
        public async Task<IActionResult> UserPage()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			UserPageVM page = new UserPageVM();
			if(user != null)
			{
				page = new UserPageVM
				{
					Fullname = page.Fullname,
					Username = page.Username,
					Email = page.Email,
					NewPassword = page.NewPassword
				};
			}
			
            return View(page);
        }
		[HttpPost]
		public async Task<IActionResult> UserPage(UserPageVM vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			if (User.Identity.Name == null) return NotFound();
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			user.Email = vm.Email;
			user.Fullname = vm.Fullname;
			user.UserName = vm.Username;
			return View();
		}
        #endregion
    }
}
