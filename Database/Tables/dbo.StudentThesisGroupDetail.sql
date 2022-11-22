CREATE TABLE [dbo].[StudentThesisGroupDetail] (
  [PK_StudentThesisGroup_ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  CONSTRAINT [PK_StudentThesisGroupDetail_PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([PK_StudentThesisGroup_ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentThesisGroupDetail]
  ADD CONSTRAINT [FK_StudentThesisGroupDetail] FOREIGN KEY ([PK_StudentThesisGroup_ID]) REFERENCES [dbo].[StudentThesisGroup] ([ID])
GO