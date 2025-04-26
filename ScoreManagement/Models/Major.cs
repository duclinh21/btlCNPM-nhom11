using System;
using System.Collections.Generic;

namespace ScoreManagement.Models
{
    public partial class Major
    {
        public Major()
        {
            Students = new HashSet<Student>();
        }

        public int MajorId { get; set; }
        public string? MajorCode { get; set; }
        public string MajorName { get; set; } = null!;
        public string? Image { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
