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

            Console.WriteLine("Seeding Roles...");
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
            var admins = new[] { "admin@lms.se", "admin2@lms.se", "bobross@lms.se" };
            Console.WriteLine("Seeding admins...");
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
                new ActivityType { Name = "Seminar", AllowUploads = false },
                new ActivityType { Name = "Assignment", AllowUploads = true }
                );
            db.SaveChanges();
            Console.WriteLine("Seeding courses...");
            var courseNames = new[] { "C#/.Net", "Java/Spring", "Web development", "Python/Django", "Javascript/Node" };
            for (int i = 0; i < courseNames.Length; i++)
            {
                db.Courses.AddOrUpdate(
                    c => c.Title,
                    new CourseModel
                    {
                        Title = courseNames[i],
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(13)
                    }
                    );
            }
            db.SaveChanges();
            Console.WriteLine("Seeding users...");
            var usercourse = db.Courses.First();
            foreach (var email in students) {
                if (db.Users.Any(u => u.UserName == email)) continue;
                var user = new ApplicationUser { UserName = email, Email = email, Course = usercourse, FirstName = email.Substring(0, email.IndexOf('@')), LastName = email.Substring(0, email.IndexOf('@'))+"sson" };
                var result = userManager.Create(user, "password");
                if (!result.Succeeded) {
                    throw new Exception(string.Join("\n", result.Errors));
                }
                userManager.AddToRole(user.Id, "User");
            }

            db.SaveChanges();
            Console.WriteLine("Seeding modules...");
            var moduleNames = new[] { "C#", ".Net", "HTML", "C++", "ASP/MVC", "Java", "Spring", "Javascript", "Rust", "Python", "Django", "Assembly", "React", "Angular" };
            const int moduleLength = 3;
            foreach (var course in db.Courses.ToList()) {
                for (int j = 0; j < 4; j++) {
                    db.Modules.AddOrUpdate(
                        c => c.Name,
                        new ModuleModel {
                            Name = moduleNames[(course.Id + j) % moduleNames.Length] + course.Id,
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
            Console.WriteLine("Seeding activities...");
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

            db.SaveChanges();
            var ext = new[] { "exe", "zip", "rar", "7z", "docx", "pdf", "tar.gz", "md", "txt", "ppx", "png", "bmp", "gif", "html", "js", "cs", "java", "jar", "py", "rs" };
            var teachnames = new[] { "Instructions", "Seminar", "Links", "Virus", "Readme", "Assignment", "Todo", "Zenbu" };
            var teachers = db.Users.Where(u => admins.Contains(u.Email)).ToList();
            Console.WriteLine("Seeding course docs....");
            foreach (var course in db.Courses.ToList()) {
                if (course.Id % 2 == 0) {
                    var filename = $"{teachnames[course.Id % teachnames.Length]}.{ext[course.Id % ext.Length]}";
                    var doc = new DocumentModel {
                        Uploader = teachers[course.Id % teachers.Count],
                        Course = course,
                        FileName = filename,
                        Path = $"Fake/Course/{course.Id}/{filename}",
                        IsHandIn = false,
                        TimeStamp = course.StartDate.AddDays(-4),
                    };
                    db.Documents.AddOrUpdate(d => d.Path, doc);
                }
            }

            db.SaveChanges();
            Console.WriteLine("Seeding module docs...");
            foreach (var module in db.Modules.ToList()) {
                if (module.Id % 2 == 0) {
                    var filename = $"{teachnames[module.Id % teachnames.Length]}.{ext[module.Id % ext.Length]}";
                    var doc = new DocumentModel {
                        Uploader = teachers[module.Id % teachers.Count],
                        Module = module,
                        FileName = filename,
                        Path = $"Module/{module.Id}/{filename}",
                        IsHandIn = false,
                        TimeStamp = module.StartDate.AddDays(-4),
                    };
                    db.Documents.AddOrUpdate(d => d.Path, doc);
                }
            }

            db.SaveChanges();
            Console.WriteLine("Seeding activity docs/hand-ins...");
            foreach (var activity in db.Activities.ToList()) {
                if (activity.Id % 2 == 0) {
                    var filename = $"{teachnames[activity.Id % teachnames.Length]}.{ext[activity.Id % ext.Length]}";
                    var doc = new DocumentModel {
                        Uploader = teachers[activity.Id % teachers.Count],
                        Activity = activity,
                        FileName = filename,
                        Path = $"Fake/Activity/{activity.Id}/{filename}",
                        IsHandIn = false,
                        TimeStamp = activity.StartDate.AddDays(-4),
                    };
                    db.Documents.AddOrUpdate(d => d.Path, doc);
                }
                if (activity.ActivityType.AllowUploads) {
                    int i = activity.Id;
                    foreach (var student in activity.Module.Course.Students.ToList()) {
                        var filename = $"{activity.Name}-Handin.{ext[i % ext.Length]}";
                        var doc = new DocumentModel {
                            Uploader = student,
                            Activity = activity,
                            FileName = filename,
                            Path = $"Fake/Activity/{activity.Id}/{filename}",
                            IsHandIn = true,
                            TimeStamp = activity.EndDate.AddDays(-2 + i++ % 4),
                        };
                        if (i % 2 != 0) {
                            doc.Feedback = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Maiores, vel aut sequi vero sapiente quidem quae odit? Explicabo quam autem consequuntur quibusdam ipsam, necessitatibus porro ipsum sequi, nihil tempore repudiandae.";
                        }
                        db.Documents.AddOrUpdate(d => d.Path, doc);
                    }
                }
            }
        }
    }
}
