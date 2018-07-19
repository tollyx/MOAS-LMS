using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models.View
{
    public class CourseViewModel
    {
        public int Id;
        public string Title;
        public string Description;
        public string StartDate;
        public string EndDate;
        public virtual IList<ApplicationUser> Students { get; set; }
    }
}