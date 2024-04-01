using BankLoginPage.Abstraction.Services;
using BankLoginPage.Abstraction.Services.TokenService;
using BankLoginPage.Implementation.Services;
using BankLoginPage.Implementation.Services.TokenService;
using BankLoginPage.Models.IdentityEntities;
using BankLoginPage.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BankLoginPage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RegisterValidator>())
                ;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<ITokenHandler, Implementation.Services.TokenService.TokenHandler>();


            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "AccessToken"; // Name of the session cookie
                options.Cookie.HttpOnly = true; // Ensure the cookie is accessible only over HTTP
                options.IdleTimeout = TimeSpan.FromMinutes(10); // Session expiration time
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer("Admin", options =>
               options.TokenValidationParameters = new()
               {//bu applicationa her kimse token ile muraciet etse o tokende bu parametrler dogrulanacaq
                   ValidateAudience = true,//olusturulacak token degerini kimlerin, hansi sitelerin/originlerin kullanacagini belirlediyimiz deyer
                   ValidateIssuer = true,// olusturulacak token deyerini kimin dagittigini belirttiyimiz alandir
                   ValidateLifetime = true,//olusturulan token degerinin suresini kontroll eden dogrulamadir
                   ValidateIssuerSigningKey = true,//uretilen token in uygulamamiza aid bir deger oldugunu ifade eden security key verisinin dogrulanmasidir
                   ValidAudience = builder.Configuration["Token:Audience"],
                   ValidIssuer = builder.Configuration["Token:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
                   LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,//to do see again jwt nin vaxti 5 deq uzanirdi, bununla 1 deq veriremse 1 deqe de bitir
                   NameClaimType = ClaimTypes.Name, // JWT uzerinde name claimine karsilik gelen degeri User.Identity.Name propertisindenn elde ede biliriz
               });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession(); // Enable session middleware

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
