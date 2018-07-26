using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models {
    public class DocumentModel {
        public int Id { get; set; }
        public string FileName { get; set; }
        public virtual ActivityModel Activity { get; set; }
        public virtual ModuleModel Module { get; set; }
        public virtual CourseModel Course { get; set; }
        public string Path { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Feedback { get; set; }
        public ApplicationUser Uploader { get; set; }
        public bool IsHandIn { get; set; }
    }
}