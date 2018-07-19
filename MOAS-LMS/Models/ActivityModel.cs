using System;


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
    }
}