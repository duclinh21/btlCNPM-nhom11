using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class ClassCourse
    {
        public int ClassCourseId { get; set; }
        public int? ClassId { get; set; }
        public int? CourseId { get; set; }
        public int? LecturerId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Course? Course { get; set; }
        public virtual Lecturer? Lecturer { get; set; }
    }
}
