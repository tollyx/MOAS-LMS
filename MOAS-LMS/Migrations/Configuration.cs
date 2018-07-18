using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOAS_LMS.Models;
using System.Linq;

namespace MOAS_LMS.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MOAS_LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MOAS_LMS.Models.ApplicationDbContext db)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Admin", "User" };

            foreach (var roleName in roleNames)
            {

                if (db.Roles.Any(r => r.Name == roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new[] { "admin@lms.se", "hannah@elev.lms.se" };
            foreach (var email in emails)
            {
                if (db.Users.Any(u => u.UserName == email)) continue;
                var user = new ApplicationUser { UserName = email, Email = email };
                var result = userManager.Create(user, "password");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            var adminUser = userManager.FindByName("admin@lms.se");
            //adminUser.FirstName = "Henrik";
            //adminUser.LastName = "Svensson";
            //adminUser.TimeOfRegistration = DateTime.Now;
            userManager.Update(adminUser);
            userManager.AddToRole(adminUser.Id, "Admin");


            for (int i = 1; i < 10; i++)
            {
                db.Courses.AddOrUpdate(
                    c => c.Title,
                    new CourseModel
                    {
                        Title = "Course" + i,
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    }
                    );
            }
        }
    }
}
