using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScoreManagement.Models
{
    public partial class Account
    {
        public Account()
        {
            Lecturers = new HashSet<Lecturer>();
            Students = new HashSet<Student>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; } = null!;
        [Display(Name = "Password")]
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Avatar { get; set; }

        public virtual ICollection<Lecturer> Lecturers { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
