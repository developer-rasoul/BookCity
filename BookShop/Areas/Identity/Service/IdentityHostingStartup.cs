using System;
using BookShop.Areas.Identity.Data;
using BookShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BookShop.Areas.Identity.IdentityHostingStartup))]
namespace BookShop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IdentityContextConnection")));

                //services.AddDefaultIdentity<BookShopUser>()
                //    .AddEntityFrameworkStores<IdentityContext>();

                services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultUI()
                .AddErrorDescriber<ApplicationIdentityErrorDescriber>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();


                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;

                    options.SignIn.RequireConfirmedEmail = true;
                });
            });
        }
}
}