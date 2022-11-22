CREATE TABLE [dbo].[Students] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Phone] [varchar](20) NOT NULL,
  [Email] [varchar](200) NOT NULL,
  [Adress] [nvarchar](400) NOT NULL,
  [Birthday] [date] NOT NULL,
  [Avatar] [varchar](200) NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [PK_StudentClass_ID] [nvarchar](50) NOT NULL,
  [PK_CourseTraining_Id] [varchar](50) NOT NULL,
  [PK_Thesis_ID] [varchar](50) NOT NULL,
  CONSTRAINT [PK_Students_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO