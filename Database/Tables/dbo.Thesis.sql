CREATE TABLE [dbo].[Thesis] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [MaxStudentNumber] [int] NOT NULL,
  [SourceCode] [nvarchar](50) NOT NULL,
  [Generalcommet ] [nvarchar](50) NOT NULL,
  [Year] [int] NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [PK_Topic_ID] [varchar](50) NOT NULL,
  [PK_Council_ID] [varchar](50) NOT NULL,
  CONSTRAINT [PK_Thesis_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Thesis]
  ADD CONSTRAINT [FK_Thesis_Council_ID] FOREIGN KEY ([PK_Council_ID]) REFERENCES [dbo].[Council] ([ID])
GO

ALTER TABLE [dbo].[Thesis]
  ADD CONSTRAINT [FK_Thesis_Topic_ID] FOREIGN KEY ([PK_Topic_ID]) REFERENCES [dbo].[Topic] ([ID])
GO