CREATE TABLE [dbo].[StudentThesisGroupDetails] (
  [StudentThesisGroupId] [varchar](50) NOT NULL,
  [StudentId] [varchar](50) NOT NULL,
  [Notes] [ntext] NULL,
  CONSTRAINT [PK_StudentThesisGroupDetail_PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([StudentThesisGroupId], [StudentId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO