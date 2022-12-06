CREATE TABLE [dbo].[Theses] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [nvarchar](100) NOT NULL,
  [MaxStudentNumber] [int] NOT NULL,
  [DocumentFile] [nvarchar](50) NOT NULL,
  [PresentationFile] [nvarchar](50) NOT NULL,
  [PdfFile] [nvarchar](50) NOT NULL,
  [SourceCode] [nvarchar](50) NOT NULL,
  [Credits] [int] NOT NULL,
  [Year] [int] NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [TopicId] [varchar](50) NOT NULL,
  [CouncilId] [varchar](50) NOT NULL,
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
  CONSTRAINT [PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Specializations_ID] FOREIGN KEY ([SpecializationId]) REFERENCES [dbo].[Specializations] ([ID])
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Specialization_ID] FOREIGN KEY ([SpecializationId]) REFERENCES [dbo].[Specialization] ([ID])
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Topics_ID] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[Topics] ([ID])
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_TrainingForms_ID] FOREIGN KEY ([TrainingFormId]) REFERENCES [dbo].[TrainingForms] ([ID])
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_TrainingLevels_ID] FOREIGN KEY ([TrainingLevelId]) REFERENCES [dbo].[TrainingLevels] ([ID])
GO