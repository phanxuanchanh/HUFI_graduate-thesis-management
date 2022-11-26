CREATE TABLE [dbo].[Theses] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NOT NULL,
  [MaxStudentNumber] [int] NOT NULL,
  [SourceCode] [nvarchar](50) NOT NULL,
  [GeneralComment ] [nvarchar](50) NOT NULL,
  [Year] [int] NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [TopicId] [varchar](50) NOT NULL,
  [CouncilId] [varchar](50) NOT NULL,
  CONSTRAINT [PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Councils_ID] FOREIGN KEY ([CouncilId]) REFERENCES [dbo].[Councils] ([ID])
GO

ALTER TABLE [dbo].[Theses]
  ADD CONSTRAINT [FK_Theses_Topics_ID] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[Topics] ([ID])
GO