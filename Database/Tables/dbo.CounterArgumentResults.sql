CREATE TABLE [dbo].[CounterArgumentResults] (
  [ThesisId] [varchar](50) NOT NULL,
  [LectureId] [varchar](50) NOT NULL,
  [Contents] [ntext] NULL,
  [ResearchMethods] [ntext] NULL,
  [ScientificResults] [ntext] NULL,
  [PracticalResults] [ntext] NULL,
  [Defects] [ntext] NULL,
  [Conclusions] [ntext] NULL,
  [Questions] [ntext] NULL,
  [Point] [decimal] NULL,
  [IsCompleted] [bit] NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_Counterargument_PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ThesisId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CounterArgumentResults]
  ADD CONSTRAINT [FK_Counterargument_FacultyStaffs_ID] FOREIGN KEY ([LectureId]) REFERENCES [dbo].[FacultyStaffs] ([ID])
GO

ALTER TABLE [dbo].[CounterArgumentResults]
  ADD CONSTRAINT [FK_CounterArguments_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID])
GO