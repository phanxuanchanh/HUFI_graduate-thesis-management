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

        public virtual DbSet<CommitteeMember> CommitteeMembers { get; set; }
        public virtual DbSet<CommitteeMemberResult> CommitteeMemberResults { get; set; }
        public virtual DbSet<CounterArgumentResult> CounterArgumentResults { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<FacultyStaff> FacultyStaffs { get; set; }
        public virtual DbSet<FacultyStaffRole> FacultyStaffRoles { get; set; }
        public virtual DbSet<ImplementationPlan> ImplementationPlans { get; set; }
        public virtual DbSet<MemberEvaluation> MemberEvaluations { get; set; }
        public virtual DbSet<MemberEvalutionPattern> MemberEvalutionPatterns { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<StudentThesisGroup> StudentThesisGroups { get; set; }
        public virtual DbSet<StudentThesisGroupDetail> StudentThesisGroupDetails { get; set; }
        public virtual DbSet<Thesis> Theses { get; set; }
        public virtual DbSet<ThesisCommittee> ThesisCommittees { get; set; }
        public virtual DbSet<ThesisCommitteeResult> ThesisCommitteeResults { get; set; }
        public virtual DbSet<ThesisRevision> ThesisRevisions { get; set; }
        public virtual DbSet<ThesisSupervisor> ThesisSupervisors { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<TrainingForm> TrainingForms { get; set; }
        public virtual DbSet<TrainingLevel> TrainingLevels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CommitteeMember>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ThesisCommitteeId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Titles)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CommitteeMembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeMembers_FacultyStaffs_ID");

                entity.HasOne(d => d.ThesisCommittee)
                    .WithMany(p => p.CommitteeMembers)
                    .HasForeignKey(d => d.ThesisCommitteeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeMembers_ThesisCommittees_ID");
            });

            modelBuilder.Entity<CommitteeMemberResult>(entity =>
            {
                entity.HasKey(e => new { e.ThesisId, e.CommitteeMemberId })
                    .HasName("PK_CouncilMembers_ID");

                entity.Property(e => e.ThesisId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CommitteeMemberId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.EvaluationId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Point).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.CommitteeMember)
                    .WithMany(p => p.CommitteeMemberResults)
                    .HasForeignKey(d => d.CommitteeMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeMemberResults_CommitteeMembers_ID");

                entity.HasOne(d => d.Evaluation)
                    .WithMany(p => p.CommitteeMemberResults)
                    .HasForeignKey(d => d.EvaluationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeMemberResults_MemberEvaluations_ID");

                entity.HasOne(d => d.Thesis)
                    .WithMany(p => p.CommitteeMemberResults)
                    .HasForeignKey(d => d.ThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeMemberResults_Theses_ID");
            });

            modelBuilder.Entity<CounterArgumentResult>(entity =>
            {
                entity.HasKey(e => e.ThesisId)
                    .HasName("PK_Counterargument_PK_Thesis_ID");

                entity.Property(e => e.ThesisId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Conclusions).HasColumnType("ntext");

                entity.Property(e => e.Contents).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Defects).HasColumnType("ntext");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.LectureId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Point).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PracticalResults).HasColumnType("ntext");

                entity.Property(e => e.Questions).HasColumnType("ntext");

                entity.Property(e => e.ResearchMethods).HasColumnType("ntext");

                entity.Property(e => e.ScientificResults).HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Lecture)
                    .WithMany(p => p.CounterArgumentResults)
                    .HasForeignKey(d => d.LectureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Counterargument_FacultyStaffs_ID");

                entity.HasOne(d => d.Thesis)
                    .WithOne(p => p.CounterArgumentResult)
                    .HasForeignKey<CounterArgumentResult>(d => d.ThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CounterArguments_Theses_ID");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

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

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Avatar)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Degree)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FacultyId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyRoleId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.FacultyStaffs)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacultyStaffs_Faculties_ID");

                entity.HasOne(d => d.FacultyRole)
                    .WithMany(p => p.FacultyStaffs)
                    .HasForeignKey(d => d.FacultyRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacultyStaffs_FacultyStaffRoles_ID ");
            });

            modelBuilder.Entity<FacultyStaffRole>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID ");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<ImplementationPlan>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Task)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.ThesisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Thesis)
                    .WithMany(p => p.ImplementationPlans)
                    .HasForeignKey(d => d.ThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImplementationPlans_Theses_ID");
            });

            modelBuilder.Entity<MemberEvaluation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.EvalutionPatternId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Point).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.EvalutionPattern)
                    .WithMany(p => p.MemberEvaluations)
                    .HasForeignKey(d => d.EvalutionPatternId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberEvaluations_MemberEvalutionPatterns_ID");
            });

            modelBuilder.Entity<MemberEvalutionPattern>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.MaxPoint).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Specialization>(entity =>
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

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.Avatar)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StudentClassId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.StudentClass)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.StudentClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

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

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DocumentFile)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LectureId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PdfFile)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PresentationFile)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SpecializationId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TopicId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TrainingFormId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TrainingLevelId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Lecture)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.LectureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Theses_FacultyStaffs_ID");

                entity.HasOne(d => d.Specialization)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.SpecializationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Theses_Specializations_ID");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Theses_Topics_ID");

                entity.HasOne(d => d.TrainingForm)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.TrainingFormId)
                    .HasConstraintName("FK_Theses_TrainingForms_ID");

                entity.HasOne(d => d.TrainingLevel)
                    .WithMany(p => p.Theses)
                    .HasForeignKey(d => d.TrainingLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Theses_TrainingLevels_ID");
            });

            modelBuilder.Entity<ThesisCommittee>(entity =>
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

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<ThesisCommitteeResult>(entity =>
            {
                entity.HasKey(e => e.ThesisId)
                    .HasName("PK_ThesisCommitteeResults_ThesisId");

                entity.Property(e => e.ThesisId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Conclusions).HasColumnType("ntext");

                entity.Property(e => e.Contents).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Point).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ThesisCommitteeId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.ThesisCommittee)
                    .WithMany(p => p.ThesisCommitteeResults)
                    .HasForeignKey(d => d.ThesisCommitteeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThesisCommitteeResults_ThesisCommittees_ID");

                entity.HasOne(d => d.Thesis)
                    .WithOne(p => p.ThesisCommitteeResult)
                    .HasForeignKey<ThesisCommitteeResult>(d => d.ThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThesisCommitteeResults_Theses_ID");
            });

            modelBuilder.Entity<ThesisRevision>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Summary).HasMaxLength(450);

                entity.Property(e => e.ThesisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Thesis)
                    .WithMany(p => p.ThesisRevisions)
                    .HasForeignKey(d => d.ThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThesisRevisions_Theses_ID");
            });

            modelBuilder.Entity<ThesisSupervisor>(entity =>
            {
                entity.HasKey(e => e.ThesisId)
                    .HasName("PK_Guide_PK_Thesis_ID");

                entity.Property(e => e.ThesisId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Attitudes).HasColumnType("ntext");

                entity.Property(e => e.Conclusions).HasMaxLength(100);

                entity.Property(e => e.Contents).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.LectureId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.Results).HasColumnType("ntext");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Lecture)
                    .WithMany(p => p.ThesisSupervisors)
                    .HasForeignKey(d => d.LectureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThesisSupervisors_FacultyStaffs_ID");

                entity.HasOne(d => d.Thesis)
                    .WithOne(p => p.ThesisSupervisor)
                    .HasForeignKey<ThesisSupervisor>(d => d.ThesisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThesisSupervisors_Theses_ID");
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

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TrainingForm>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TrainingLevel>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
