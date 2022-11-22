CREATE TABLE [dbo].[Counterargument] (
  [PK_Thesis_ID] [varchar](50) NOT NULL,
  [PK_Lecturers_ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  [Comment] [nvarchar](50) NOT NULL,
  [Feedbackpoints ] [int] NOT NULL,
  CONSTRAINT [PK_Counterargument_PK_Thesis_ID] PRIMARY KEY CLUSTERED ([PK_Thesis_ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Counterargument]
  ADD CONSTRAINT [FK_Counterargument_Lecturers_ID] FOREIGN KEY ([PK_Lecturers_ID]) REFERENCES [dbo].[Lecturers] ([ID])
GO

ALTER TABLE [dbo].[Counterargument]
  ADD CONSTRAINT [FK_Counterargument_Thesis_ID] FOREIGN KEY ([PK_Thesis_ID]) REFERENCES [dbo].[Thesis] ([ID])
GO