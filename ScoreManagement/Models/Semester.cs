using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class Semester
    {
        public Semester()
        {
            Classes = new HashSet<Class>();
            StudentsCourses = new HashSet<StudentsCourse>();
        }

        public int SemesterId { get; set; }
        public string? SemesterCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<StudentsCourse> StudentsCourses { get; set; }
    }
}
