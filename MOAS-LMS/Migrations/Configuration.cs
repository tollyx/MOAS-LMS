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

            var students = new[] { "hannah@elev.lms.se", "bert@elev.lms.se", "gustav@elev.lms.se" };
            var admins = new[] { "admin@lms.se", "admin2@lms.se" };
            foreach (var email in admins)
            {
                if (db.Users.Any(u => u.UserName == email)) continue;
                var user = new ApplicationUser { UserName = email, Email = email };
                var result = userManager.Create(user, "password");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
                userManager.AddToRole(user.Id, "Admin");
            }

            db.ActivityTypes.AddOrUpdate(
                at => at.Name,
                new ActivityType { Name = "E-Learning", AllowUploads = false },
                new ActivityType { Name = "Föreläsning", AllowUploads = false },
                new ActivityType { Name = "Inlämningsuppgift", AllowUploads = true }
                );
            db.SaveChanges();

            for (int i = 1; i < 10; i++)
            {
                db.Courses.AddOrUpdate(
                    c => c.Title,
                    new CourseModel
                    {
                        Title = "Course" + i,
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(13)
                    }
                    );
            }
            db.SaveChanges();

            var usercourse = db.Courses.First();
            foreach (var email in students) {
                if (db.Users.Any(u => u.UserName == email)) continue;
                var user = new ApplicationUser { UserName = email, Email = email, Course = usercourse };
                var result = userManager.Create(user, "password");
                if (!result.Succeeded) {
                    throw new Exception(string.Join("\n", result.Errors));
                }
                userManager.AddToRole(user.Id, "User");
            }

            db.SaveChanges();
            const int moduleLength = 3;
            foreach (var course in db.Courses.ToList()) {
                for (int j = 0; j < 4; j++) {
                    db.Modules.AddOrUpdate(
                        c => c.Name,
                        new ModuleModel {
                            Name = $"Module{course.Id}-{j}",
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam",
                            StartDate = course.StartDate.AddDays(j * moduleLength),
                            EndDate = course.StartDate.AddDays((j + 1) * moduleLength),
                            Course = course
                        }
                    );
                }
            }

            db.SaveChanges();
            int activityi = 0;
            const int activitylength = 1;
            var actTypes = db.ActivityTypes.ToList();
            foreach (var module in db.Modules.ToList())
            {
                foreach (var act in actTypes)
                {
                    db.Activities.AddOrUpdate(
                        a => a.Name,
                        new ActivityModel
                        {
                            Name = act.Name + module.Id,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam",
                            StartDate = module.StartDate.AddDays(activityi * activitylength),
                            EndDate = module.StartDate.AddDays((activityi + 1) * activitylength),
                            ActivityType = act,
                            Module = module,
                        }
                    );
                    activityi++;
                }
            }
        }
    }
}
