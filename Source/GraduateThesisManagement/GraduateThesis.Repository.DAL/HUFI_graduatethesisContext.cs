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
        public virtual DbSet<CouncilMember> CouncilMembers { get; set; }
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
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Chairman)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CouncilPoint).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<CouncilMember>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CouncilId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("councilId");
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

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DoresearchQuantiity).HasColumnName("DoresearchQuantiity ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PkResearchId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PK_Research_ID");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Dean)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<FacultyStaff>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.FacultyId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);
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

                entity.Property(e => e.Description)
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
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CourseTrainingId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Salt).HasMaxLength(100);

                entity.Property(e => e.StudentClassId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.StudentClass)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.StudentClassId)
                    .HasConstraintName("FK_Students_StudentClasses_ID");
            });

            modelBuilder.Entity<StudentClass>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.FacultyId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.StudentClasses)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentClasses_Faculties_ID");
            });

            modelBuilder.Entity<StudentThesisGroup>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes).HasColumnType("ntext");

                entity.Property(e => e.ThesisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Thesis)
                    .WithMany(p => p.StudentThesisGroups)
                    .HasForeignKey(d => d.ThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentThesisGroups_Theses_ID");
            });

            modelBuilder.Entity<StudentThesisGroupDetail>(entity =>
            {
                entity.HasKey(e => new { e.StudentThesisGroupId, e.StudentId })
                    .HasName("PK_StudentThesisGroupDetail_PK_StudentThesisGroup_ID");

                entity.Property(e => e.StudentThesisGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).HasColumnType("ntext");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentThesisGroupDetails)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentThesisGroupDetails_Students_ID");

                entity.HasOne(d => d.StudentThesisGroup)
                    .WithMany(p => p.StudentThesisGroupDetails)
                    .HasForeignKey(d => d.StudentThesisGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentThesisGroupDetails_StudentThesisGroups_ID");
            });

            modelBuilder.Entity<Thesis>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CouncilId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GeneralComment)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("GeneralComment ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TopicId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Council)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.CouncilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Theses_Councils_ID");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Theses_Topics_ID");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
