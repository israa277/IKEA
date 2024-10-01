using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.BLL.Services.Employees;
using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.DAL.Persistence.Data;
using LinkDev.IKEA.DAL.Persistence.Repositories.Departments;
using LinkDev.IKEA.DAL.Persistence.Repositories.Employees;
using LinkDev.IKEA.DAL.Persistence.Repositories.Employess;
using LinkDev.IKEA.DAL.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LinkDev.IKEA.PL
{
    public class Program
    {
        //Entry Point
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

			//builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService,EmployeeServices>();
			//builder.Services.AddTransient<IAttachmentService, AttachmentService>();

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>((option) =>
            {
                option.Password.RequiredLength = 5;
                option.Password.RequireDigit = true;
                option.Password.RequireNonAlphanumeric = true; // *#%&
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequiredUniqueChars = 1;
                option.User.RequireUniqueEmail = true;
                option.Lockout.AllowedForNewUsers = true;
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            }).AddEntityFrameworkStores<ApplicationDbContext>();
			//option.User.AllowedUserNameCharacters = "asmdma;asdms";

			//builder.Services.AddScoped<ApplicationDbContext>();
			//builder.Services.AddScoped<DbContextOptions<ApplicationDbContext>>((ServiceProvider) =>
			//{
			//    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			//    optionsBuilder.UseSqlServer("");
			//    var options = optionsBuilder.Options;
			//    return options;
			//});
			#endregion
			var app = builder.Build();

            #region Configure Kestrel Middlewares
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            #endregion

            app.Run();
        }
    }
}
