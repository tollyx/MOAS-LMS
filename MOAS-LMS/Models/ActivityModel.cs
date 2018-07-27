using System;
using System.Collections.Generic;
using System.Linq;

namespace MOAS_LMS.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public virtual ModuleModel Module { get; set; }
        public virtual ICollection<DocumentModel> Documents { get; set; }

        public bool HasUserAccess(ApplicationUser user) {
            return Module?.HasUserAccess(user) ?? false;
        }
    }
}