using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ScoreManagement.Models
{
    public partial class Project_PRN221Context : DbContext
    {
        public static Project_PRN221Context Ins = new Project_PRN221Context();
        public Project_PRN221Context()
        {
            if (Ins == null) Ins = this;
        }

        public Project_PRN221Context(DbContextOptions<Project_PRN221Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<ClassCourse> ClassCourses { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Lecturer> Lecturers { get; set; } = null!;
        public virtual DbSet<Major> Majors { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentClass> StudentClasses { get; set; } = null!;
        public virtual DbSet<StudentsCourse> StudentsCourses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("value"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Accounts__536C85E48EDB8BC5")
                    .IsUnique();

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.PasswordHash).HasMaxLength(256);

                entity.Property(e => e.Role).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasIndex(e => e.ClassCode, "UQ__Classes__2ECD4A554CA656ED")
                    .IsUnique();

                entity.Property(e => e.ClassCode).HasMaxLength(10);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__Classes__Semeste__534D60F1");
            });

            modelBuilder.Entity<ClassCourse>(entity =>
            {
                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassCourses)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__ClassCour__Class__5070F446");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.ClassCourses)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__ClassCour__Cours__5165187F");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.ClassCourses)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__ClassCour__Lectu__52593CB8");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.CourseCode, "UQ__Courses__FC00E000FA3C03B8")
                    .IsUnique();

                entity.Property(e => e.CourseCode).HasMaxLength(10);

                entity.Property(e => e.CourseName).HasMaxLength(100);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.Property(e => e.Status).HasMaxLength(100);

                entity.HasOne(d => d.StudentCourse)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.StudentCourseId)
                    .HasConstraintName("FK__Grades__StudentC__5441852A");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.LecturerName).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Lecturers)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Lecturers__Accou__5535A963");
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.HasIndex(e => e.MajorCode, "UQ__Majors__64E58F94A42EED8D")
                    .IsUnique();

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.MajorCode).HasMaxLength(10);

                entity.Property(e => e.MajorName).HasMaxLength(100);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.HasIndex(e => e.SemesterCode, "UQ__Semester__513151C95CA465BA")
                    .IsUnique();

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.SemesterCode).HasMaxLength(10);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.StudentCode, "UQ__Students__1FC88604E342F810")
                    .IsUnique();

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.StudentCode).HasMaxLength(10);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Students__Accoun__5812160E");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.MajorId)
                    .HasConstraintName("FK__Students__MajorI__59063A47");
            });

            modelBuilder.Entity<StudentClass>(entity =>
            {
                entity.HasOne(d => d.Class)
                    .WithMany(p => p.StudentClasses)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__StudentCl__Class__5629CD9C");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentClasses)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__StudentCl__Stude__571DF1D5");
            });

            modelBuilder.Entity<StudentsCourse>(entity =>
            {
                entity.HasKey(e => e.StudentCourseId)
                    .HasName("PK__Students__7E3E2F922B13AB35");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.StudentsCourses)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__StudentsC__Class__59FA5E80");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.StudentsCourses)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__StudentsC__Cours__5AEE82B9");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.StudentsCourses)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__StudentsC__Lectu__5BE2A6F2");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.StudentsCourses)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__StudentsC__Semes__5CD6CB2B");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentsCourses)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__StudentsC__Stude__5DCAEF64");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
