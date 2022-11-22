CREATE TABLE [dbo].[CourseTraining] (
  [Id] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [ShoolYear] [int] NOT NULL,
  CONSTRAINT [PK_CourseTraining_Id] PRIMARY KEY CLUSTERED ([Id])
)
ON [PRIMARY]
GO