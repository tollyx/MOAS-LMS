using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models.View
{
    public class CourseViewModel
    {
        public int Id;
        [Display(Name = "Course")]
        public string Title;
        public string Description;

        [Display(Name = "Start date")]
        public string StartDate;

        [Display(Name = "End date")]
        public string EndDate;

        public IList<ApplicationUser> Students { get; set; }
        public IList<ModuleModel> Modules { get; set; }
        public IList<DocumentModel> Documents { get; set; }
    }
}