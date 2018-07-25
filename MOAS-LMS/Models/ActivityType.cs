using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool AllowUploads { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}