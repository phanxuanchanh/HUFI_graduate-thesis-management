CREATE TABLE [dbo].[Students] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Phone] [varchar](20) NOT NULL,
  [Email] [varchar](200) NOT NULL,
  [Adress] [nvarchar](400) NOT NULL,
  [Birthday] [date] NOT NULL,
  [Avatar] [varchar](200) NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  [StudentClassId] [varchar](50) NOT NULL,
  [CourseTrainingId] [varchar](50) NOT NULL,
  [ThesisId] [varchar](50) NOT NULL,
  [Password] [nvarchar](100) NOT NULL,
  [Salt] [nvarchar](100) NOT NULL,
  CONSTRAINT [PK_Students_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO