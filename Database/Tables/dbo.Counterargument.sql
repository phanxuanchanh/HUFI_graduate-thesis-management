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