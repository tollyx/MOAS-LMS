﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models
{
    public class ModuleModel
    {
        public int Id{ get; set; }
        public string Name{ get; set; }
        public string Description{ get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate{ get; set; }
        public virtual CourseModel Course { get; set; }
        public virtual ICollection<ActivityModel> Activities { get; set; }
        public virtual ICollection<DocumentModel> Documents { get; set; }

        public bool HasUserAccess(ApplicationUser user) {
            return Course?.HasUserAccess(user) ?? false;
        }
    }
}