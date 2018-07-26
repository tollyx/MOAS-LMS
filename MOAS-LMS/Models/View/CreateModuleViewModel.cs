using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOAS_LMS.Models.View {
    public class CreateModuleViewModel {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}