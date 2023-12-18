using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationPustok.Context;
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
				opt.SignIn.RequireConfirmedEmail = false;
				opt.User.RequireUniqueEmail = true;
				opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789._";
				opt.Lockout.MaxFailedAccessAttempts = 5;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequiredLength = 4;
			}).AddDefaultTokenProviders().AddEntityFrameworkStores<PustokDbContext>();
			builder.Services.AddSession(); 


            builder.Services.AddScoped<LayoutService>();
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
           
            app.UseStaticFiles();

            app.UseAuthorization();
            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Slider}/{id?}"
               );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            PathConstants.RootPath = builder.Environment.WebRootPath;

            app.Run();
        }
    }
}