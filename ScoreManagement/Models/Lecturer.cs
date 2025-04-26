using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            ClassCourses = new HashSet<ClassCourse>();
            StudentsCourses = new HashSet<StudentsCourse>();
        }

        public int LecturerId { get; set; }
        public string? LecturerName { get; set; }
        public int? AccountId { get; set; }
        public DateTime? Dob { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<ClassCourse> ClassCourses { get; set; }
        public virtual ICollection<StudentsCourse> StudentsCourses { get; set; }
    }
}
