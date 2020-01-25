 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Models
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole,string, IdentityUserClaim<string>
        , ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>().ToTable("AspNetRoles").ToTable("AppRoles");
            builder.Entity<ApplicationUserRole>().ToTable("AppUserRoles");

            builder.Entity<ApplicationUserRole>().HasOne(a => a.Role).WithMany(a => a.UserRoles).HasForeignKey(a => a.RoleId);

            builder.Entity<ApplicationUser>().ToTable("AppUsers");

            builder.Entity<ApplicationUserRole>().HasOne(a => a.User).WithMany(a => a.UserRoles).HasForeignKey(a => a.UserId);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
