using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class HUFI_graduatethesisContext : DbContext
    {
        public HUFI_graduatethesisContext()
        {
        }

        public HUFI_graduatethesisContext(DbContextOptions<HUFI_graduatethesisContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Council> Councils { get; set; }
        public virtual DbSet<Counterargument> Counterarguments { get; set; }
        public virtual DbSet<CourseTraining> CourseTrainings { get; set; }
        public virtual DbSet<Doresearch> Doresearches { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<FacultyStaff> FacultyStaffs { get; set; }
        public virtual DbSet<Guide> Guides { get; set; }
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<Research> Researches { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<StudentThesisGroup> StudentThesisGroups { get; set; }
        public virtual DbSet<StudentThesisGroupDetail> StudentThesisGroupDetails { get; set; }
        public virtual DbSet<Thesis> Theses { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Council>(entity =>
            {
                entity.ToTable("Council");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Chairman)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Counterargument>(entity =>
            {
                entity.HasKey(e => e.PkThesisId)
                    .HasName("PK_Counterargument_PK_Thesis_ID");

                entity.ToTable("Counterargument");

                entity.Property(e => e.PkThesisId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Thesis_ID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Feedbackpoints).HasColumnName("Feedbackpoints ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PkLecturersId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Lecturers_ID");

                entity.HasOne(d => d.PkLecturers)
                    .WithMany(p => p.Counterarguments)
                    .HasForeignKey(d => d.PkLecturersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Counterargument_Lecturers_ID");

                entity.HasOne(d => d.PkThesis)
                    .WithOne(p => p.Counterargument)
                    .HasForeignKey<Counterargument>(d => d.PkThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Counterargument_Thesis_ID");
            });

            modelBuilder.Entity<CourseTraining>(entity =>
            {
                entity.ToTable("CourseTraining");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Doresearch>(entity =>
            {
                entity.HasKey(e => e.PkLecturersId)
                    .HasName("PK_Doresearch_ID");

                entity.ToTable("Doresearch");

                entity.Property(e => e.PkLecturersId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Lecturers_ID");

                entity.Property(e => e.DoresearchQuantiity).HasColumnName("DoresearchQuantiity ");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PkResearchId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Research_ID");

                entity.HasOne(d => d.PkLecturers)
                    .WithOne(p => p.Doresearch)
                    .HasForeignKey<Doresearch>(d => d.PkLecturersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doresearch_Lecturers_ID");

                entity.HasOne(d => d.PkResearch)
                    .WithMany(p => p.Doresearches)
                    .HasForeignKey(d => d.PkResearchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doresearch_Research_ID");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Faculty");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Dean)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FacultyStaff>(entity =>
            {
                entity.ToTable("FacultyStaff");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PkFacultyId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Faculty_ID");

                entity.HasOne(d => d.PkFaculty)
                    .WithMany(p => p.FacultyStaffs)
                    .HasForeignKey(d => d.PkFacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacultyStaff_Faculty_ID");
            });

            modelBuilder.Entity<Guide>(entity =>
            {
                entity.HasKey(e => e.PkThesisId)
                    .HasName("PK_Guide_PK_Thesis_ID");

                entity.ToTable("Guide");

                entity.Property(e => e.PkThesisId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Thesis_ID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PkLecturersId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Lecturers_ID");

                entity.HasOne(d => d.PkThesis)
                    .WithOne(p => p.Guide)
                    .HasForeignKey<Guide>(d => d.PkThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guide_Lecturers_ID");

                entity.HasOne(d => d.PkThesisNavigation)
                    .WithOne(p => p.Guide)
                    .HasForeignKey<Guide>(d => d.PkThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guide_Thesis_ID");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Avatar)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Degree)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Research>(entity =>
            {
                entity.ToTable("Research");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.Avatar)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PkStudentClassId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("PK_StudentClass_ID");

                entity.Property(e => e.PkThesisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Thesis_ID");

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.PkStudentClass)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.PkStudentClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_StudentClass_ID");

                entity.HasOne(d => d.PkThesis)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.PkThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_Thesis_ID");
            });

            modelBuilder.Entity<StudentClass>(entity =>
            {
                entity.ToTable("StudentClass");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PkCourseTrainingId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_CourseTraining_Id");

                entity.Property(e => e.PkFacultyId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Faculty_ID");

                entity.HasOne(d => d.PkCourseTraining)
                    .WithMany(p => p.StudentClasses)
                    .HasForeignKey(d => d.PkCourseTrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentClass_CourseTraining_Id");

                entity.HasOne(d => d.PkFaculty)
                    .WithMany(p => p.StudentClasses)
                    .HasForeignKey(d => d.PkFacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentClass_Faculty_ID");
            });

            modelBuilder.Entity<StudentThesisGroup>(entity =>
            {
                entity.ToTable("StudentThesisGroup");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PkStudentsId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Students_ID");

                entity.Property(e => e.PkThesisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Thesis_ID");

                entity.HasOne(d => d.PkStudents)
                    .WithMany(p => p.StudentThesisGroups)
                    .HasForeignKey(d => d.PkStudentsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentThesisGroup_Students_ID");

                entity.HasOne(d => d.PkThesis)
                    .WithMany(p => p.StudentThesisGroups)
                    .HasForeignKey(d => d.PkThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentThesisGroup_Thesis_ID");
            });

            modelBuilder.Entity<StudentThesisGroupDetail>(entity =>
            {
                entity.HasKey(e => e.PkStudentThesisGroupId)
                    .HasName("PK_StudentThesisGroupDetail_PK_StudentThesisGroup_ID");

                entity.ToTable("StudentThesisGroupDetail");

                entity.Property(e => e.PkStudentThesisGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_StudentThesisGroup_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PkStudentThesisGroup)
                    .WithOne(p => p.StudentThesisGroupDetail)
                    .HasForeignKey<StudentThesisGroupDetail>(d => d.PkStudentThesisGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentThesisGroupDetail");
            });

            modelBuilder.Entity<Thesis>(entity =>
            {
                entity.ToTable("Thesis");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Generalcommet)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Generalcommet ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PkCouncilId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Council_ID");

                entity.Property(e => e.PkTopicId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Topic_ID");

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PkCouncil)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.PkCouncilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Thesis_Council_ID");

                entity.HasOne(d => d.PkTopic)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.PkTopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Thesis_Topic_ID");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
