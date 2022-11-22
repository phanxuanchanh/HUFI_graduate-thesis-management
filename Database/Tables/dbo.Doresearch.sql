CREATE TABLE [dbo].[Doresearch] (
  [PK_Lecturers_ID] [varchar](50) NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  [DoresearchQuantiity ] [int] NOT NULL,
  [PK_Research_ID] [varchar](50) NOT NULL,
  CONSTRAINT [PK_Doresearch_ID] PRIMARY KEY CLUSTERED ([PK_Lecturers_ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Doresearch]
  ADD CONSTRAINT [FK_Doresearch_Lecturers_ID] FOREIGN KEY ([PK_Lecturers_ID]) REFERENCES [dbo].[Lecturers] ([ID])
GO

ALTER TABLE [dbo].[Doresearch]
  ADD CONSTRAINT [FK_Doresearch_Research_ID] FOREIGN KEY ([PK_Research_ID]) REFERENCES [dbo].[Research] ([ID])
GO