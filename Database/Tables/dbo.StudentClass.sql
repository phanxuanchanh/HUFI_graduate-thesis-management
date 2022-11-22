CREATE TABLE [dbo].[StudentClass] (
  [ID] [nvarchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [nvarchar](200) NOT NULL,
  [StudentQuantity] [int] NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [PK_Faculty_ID] [varchar](50) NOT NULL,
  [PK_CourseTraining_Id] [varchar](50) NOT NULL,
  CONSTRAINT [PK_StudentClass_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentClass]
  ADD CONSTRAINT [FK_StudentClass_CourseTraining_Id] FOREIGN KEY ([PK_CourseTraining_Id]) REFERENCES [dbo].[CourseTraining] ([Id])
GO

ALTER TABLE [dbo].[StudentClass]
  ADD CONSTRAINT [FK_StudentClass_Faculty_ID] FOREIGN KEY ([PK_Faculty_ID]) REFERENCES [dbo].[Faculty] ([ID])
GO