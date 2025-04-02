using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Context;
using DataAccessLayer.Repository.IRepository;
using DataAccessLayer.Repository;
using BusinessLayer.Services.IServices;
using BusinessLayer.Services;
using BusinessLayer.Manager.IManager;
using BusinessLayer.Manager;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Stripe;


namespace E_Commerce_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            // Register DBContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
            option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequiredLength = 4;
                option.Password.RequireUppercase = false;

                option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

         

            builder.Services.AddSession();


            #region Register Repositories 

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            #endregion

            #region Register Managers

            builder.Services.AddScoped<IProductManager,ProductManager>();
            builder.Services.AddScoped<ICategoryWithProductManager, CategoryWithProductManager>();
            builder.Services.AddScoped<IRegistrationManager,RegisterManager>();

            #endregion

            #region Register Services

            builder.Services.AddScoped<IFileServices, FileServices>();

            #endregion

            var stripeSettings = builder.Configuration.GetSection("Stripe");

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
            StripeConfiguration.ApiKey = stripeSettings["SecretKey"];
            app.UseAuthorization();

            app.UseSession();

            app.MapControllers();

            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
