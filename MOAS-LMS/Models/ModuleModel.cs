using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models
{
    public class ModuleModel
    {
        public int Id;
        public string Name;
        public string Description;
        public DateTime StartDate;
        public DateTime EndDate;
        public virtual CourseModel Course { get; set; }
        public virtual IEnumerable<ActivityModel> Activities { get; set; }

    }
}