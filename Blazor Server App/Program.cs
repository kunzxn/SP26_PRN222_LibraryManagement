using Blazor_Server_App.Components;
using GalleryBusiness;
using GalleryDataAccess.Services;
using GalleryRepositories.Implementations;
using GalleryRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Server_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Register DbContext
            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();

            // Register Services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<BookService>();
            builder.Services.AddScoped<BorrowService>();
            builder.Services.AddScoped<Blazor_Server_App.Services.UserSession>();

            var app = builder.Build();

            // Auto-create database without migrations
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                dbContext.Database.EnsureCreated();

                // Robustly re-sync admin password to 'admin123' if it diverged
                var admin = dbContext.Users.FirstOrDefault(u => u.Email == "admin@university.edu");
                if (admin != null)
                {
                    bool isPasswordValid = false;
                    try
                    {
                        isPasswordValid = BCrypt.Net.BCrypt.Verify("admin123", admin.Password);
                    }
                    catch { /* Invalid hash format */ }

                    if (!isPasswordValid)
                    {
                        admin.Password = BCrypt.Net.BCrypt.HashPassword("admin123");
                        dbContext.SaveChanges();
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
