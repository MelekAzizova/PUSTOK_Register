using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationPustok.Context;
using WebApplicationPustok.ExternalService.Impliments;
using WebApplicationPustok.ExternalService.Interfaces;
using WebApplicationPustok.Helpers;
using WebApplicationPustok.Models;


namespace WebApplicationPustok
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

             builder.Services.AddControllersWithViews();
           

            builder.Services.AddDbContext<PustokDbContext>(options =>
            
            options.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"))).AddIdentity<AppUser, IdentityRole>(opt => {
				opt.SignIn.RequireConfirmedEmail = true;
				opt.User.RequireUniqueEmail = true;
				opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789._";
				opt.Lockout.MaxFailedAccessAttempts = 5;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequiredLength = 4;
			}).AddDefaultTokenProviders().AddEntityFrameworkStores<PustokDbContext>();
			builder.Services.AddSession();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Auth/Login");
                options.LogoutPath = new PathString("/Auth/Logout");
                options.AccessDeniedPath = new PathString("/Home/AccessDenied");
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromDays(30);
			});


			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<LayoutService>();
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
            app.UseHttpsRedirection();
           
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}"
               );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            PathConstants.RootPath = builder.Environment.WebRootPath;

            app.Run();
        }
    }
}