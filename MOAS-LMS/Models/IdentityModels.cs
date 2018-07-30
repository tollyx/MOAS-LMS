using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MOAS_LMS.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser {

        public virtual CourseModel Course { get; set; }
        public virtual ICollection<DocumentModel> Documents { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName
        {
            get
            {
                return
                    FirstName + " " + LastName;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        public DbSet<ActivityType> ActivityTypes { get; set; } 
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<ModuleModel> Modules { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }
        public DbSet<DocumentModel> Documents { get; set; }
    }
}
