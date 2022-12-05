CREATE TABLE [dbo].[GuideResults] (
  [ThesisId] [varchar](50) NOT NULL,
  [LecturerId] [varchar](50) NOT NULL,
  [Contents] [ntext] NULL,
  [Attitudes] [ntext] NULL,
  [Results] [ntext] NULL,
  [Conclusions] [nvarchar](100) NULL,
  [IsCompleted] [bit] NOT NULL,
  [Point] [int] NULL,
  [Notes] [nvarchar](200) NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_Guide_PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ThesisId], [LecturerId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[GuideResults]
  ADD CONSTRAINT [FK_Guide_FacultyStaffs_ID] FOREIGN KEY ([LecturerId]) REFERENCES [dbo].[FacultyStaffs] ([ID])
GO

ALTER TABLE [dbo].[GuideResults]
  ADD CONSTRAINT [FK_Guides_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID])
GO