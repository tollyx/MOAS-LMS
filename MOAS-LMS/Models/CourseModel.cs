using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models {
    public class CourseModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ICollection<ModuleModel> Modules { get; set; }
        public virtual ICollection<DocumentModel> Documents { get; set; }
    }
}