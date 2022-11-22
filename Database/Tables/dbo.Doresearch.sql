CREATE TABLE [dbo].[Doresearch] (
  [PK_Lecturers_ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  [DoresearchQuantiity ] [int] NOT NULL,
  [PK_Research_ID] [varchar](50) NOT NULL,
  CONSTRAINT [PK_Doresearch_ID] PRIMARY KEY CLUSTERED ([PK_Lecturers_ID])
)
ON [PRIMARY]
GO