CREATE TABLE [dbo].[Lecturers] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [Sex] [char](10) NOT NULL,
  [Phone] [int] NOT NULL,
  [Adress] [nvarchar](50) NOT NULL,
  [Email] [nvarchar](50) NOT NULL,
  [Birthday] [date] NOT NULL,
  [Avatar] [nvarchar](50) NOT NULL,
  [Position] [nvarchar](50) NOT NULL,
  [Degree] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  CONSTRAINT [PK_Lecturers_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO