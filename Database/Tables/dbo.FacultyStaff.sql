CREATE TABLE [dbo].[FacultyStaff] (
  [ID] [varchar](50) NOT NULL,
  [PK_Faculty_ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](200) NOT NULL,
  CONSTRAINT [PK_FacultyStaff_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO