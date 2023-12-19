using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApplicationPustok.Models;

namespace WebApplicationPustok.Context
{
    public class PustokDbContext:IdentityDbContext
    {
        

       
        public PustokDbContext(DbContextOptions<PustokDbContext> opt) : base(opt) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagProduct> TagsProduct { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>()
                .HasData(new Setting
                {
                    Address = "Baku, Ayna Sultanova st. 221",
                    Email = "melek.azizova47@gmail.com",
                    Number = "+994707094535",
                  
                    Logo = "assets/img/logo.png",
                    AccountIcon = "<i class='fa fa-user-o'></i>",
                    Id = 1
                });
            base.OnModelCreating(modelBuilder);
        }
        


    }
}
