using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<Project_PRN221Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("value")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Account/Login");
                return Task.CompletedTask;
            });
            app.Run();
        }
    }
}