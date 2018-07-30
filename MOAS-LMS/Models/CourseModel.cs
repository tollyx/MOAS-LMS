using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models {
    public class CourseModel {
        public int Id { get; set; }

        [Display(Name = "Course name")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }
        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ICollection<ModuleModel> Modules { get; set; }
        public virtual ICollection<DocumentModel> Documents { get; set; }

        public bool HasUserAccess(ApplicationUser user) {
            return this == user.Course;
        }
    }
}