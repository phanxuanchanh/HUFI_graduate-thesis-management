CREATE TABLE [dbo].[Guide] (
  [PK_Thesis_ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [GuidePoint] [int] NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [Comment] [nvarchar](50) NOT NULL,
  [PK_Lecturers_ID] [varchar](50) NOT NULL,
  CONSTRAINT [PK_Guide_PK_Thesis_ID] PRIMARY KEY CLUSTERED ([PK_Thesis_ID])
)
ON [PRIMARY]
GO