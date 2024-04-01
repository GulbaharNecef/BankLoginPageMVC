using BankLoginPage.Models;
using BankLoginPage.Models.IdentityEntities;
using BankLoginPage.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankLoginPage
{
    public class ApplicationDbContext:IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        //public DbSet<LoginModel> LoginModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<RegisterViewModel>().HasNoKey();
        }
        //public DbSet<BankLoginPage.Models.User> UserCreateModel { get; set; } = default!;
    }
}
