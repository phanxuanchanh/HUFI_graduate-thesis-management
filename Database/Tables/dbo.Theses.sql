CREATE TABLE [dbo].[Theses] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NOT NULL,
  [MaxStudentNumber] [int] NOT NULL,
  [DocumentFile] [nvarchar](50) NULL,
  [PresentationFile] [nvarchar](50) NULL,
  [PdfFile] [nvarchar](50) NULL,
  [SourceCode] [nvarchar](50) NULL,
  [Credits] [int] NOT NULL,
  [Year] [nvarchar](100) NOT NULL,
  [Notes] [nvarchar](200) NULL,
  [TopicId] [varchar](50) NOT NULL,
  [TrainingFormId] [varchar](50) NULL,
  [TrainingLevelId] [varchar](50) NOT NULL,
  [IsApproved] [bit] NOT NULL,
  [IsNew] [bit] NOT NULL,
  [InProgess] [bit] NOT NULL,
  [Finished] [bit] NOT NULL,
  [SpecializationId] [varchar](50) NOT NULL,
  [DateFrom] [datetime] NOT NULL,
  [DateTo] [datetime] NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  [LectureId] [varchar](50) NOT NULL,
  [Semester] [int] NOT NULL,
  [ThesisGroupId] [varchar](50) NULL,
  [IsRejected] [bigint] NULL,
  [IsPublished] [bigint] NULL,
  CONSTRAINT [PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_FacultyStaffs_ID] FOREIGN KEY ([LectureId]) REFERENCES [dbo].[FacultyStaffs] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Specializations_ID] FOREIGN KEY ([SpecializationId]) REFERENCES [dbo].[Specializations] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_ThesisGroups_ID] FOREIGN KEY ([ThesisGroupId]) REFERENCES [dbo].[ThesisGroups] ([ID]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Topics_ID] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[Topics] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_TrainingForms_ID] FOREIGN KEY ([TrainingFormId]) REFERENCES [dbo].[TrainingForms] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_TrainingLevels_ID] FOREIGN KEY ([TrainingLevelId]) REFERENCES [dbo].[TrainingLevels] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO