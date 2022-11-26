CREATE TABLE [dbo].[FacultyStaffs] (
  [ID] [varchar](50) NOT NULL,
  [FacultyId] [varchar](50) NOT NULL,
  [FullName] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  CONSTRAINT [PK_FacultyStaff_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO