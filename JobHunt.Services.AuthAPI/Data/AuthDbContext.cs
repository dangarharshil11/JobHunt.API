using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.AuthAPI.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Initalizing the Auth Database with Roles and Admin User
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var EmployerRoleId = "e5ff41ac-98fa-4003-950d-d5bbde128546";
            var JobSeekerRoleId = "f893dcc6-ed20-4acc-bbaf-b65873d368ae";
            var AdminRoleId = "2da7c8ca-1132-4c52-b78f-b7cc2da92139";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = JobSeekerRoleId,
                    Name = SD.RoleJobSeeker,
                    NormalizedName = SD.RoleJobSeeker.ToUpper(),
                    ConcurrencyStamp = JobSeekerRoleId
                },
                new IdentityRole()
                {
                    Id = EmployerRoleId,
                    Name = SD.RoleEmployer,
                    NormalizedName = SD.RoleEmployer.ToUpper(),
                    ConcurrencyStamp = EmployerRoleId
                },
                new IdentityRole()
                {
                    Id = AdminRoleId,
                    Name = SD.RoleAdmin,
                    NormalizedName = SD.RoleAdmin.ToUpper(),
                    ConcurrencyStamp = AdminRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);


            var adminUserId = "1f611e4b-35c2-4061-916e-a64c93b3b745";
            var admin = new ApplicationUser()
            {
                Id = adminUserId,
                FullName = "Admin User",
                UserName = "demoadmin@email.com",
                Email = "demoadmin@email.com",
                NormalizedEmail = "demoadmin@email.com".ToUpper(),
                NormalizedUserName = "demoadmin@email.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<ApplicationUser>().HasData(admin);


            var adminRole = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = AdminRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = EmployerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = JobSeekerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRole);
        }
    }
}
