CREATE TABLE [dbo].[StudentThesisGroup] (
  [ID] [varchar](50) NOT NULL,
  [PK_Students_ID] [varchar](50) NOT NULL,
  [PK_Thesis_ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [StudentQuantity] [int] NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  CONSTRAINT [PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentThesisGroup]
  ADD CONSTRAINT [FK_StudentThesisGroup_Students_ID] FOREIGN KEY ([PK_Students_ID]) REFERENCES [dbo].[Students] ([ID])
GO

ALTER TABLE [dbo].[StudentThesisGroup]
  ADD CONSTRAINT [FK_StudentThesisGroup_Thesis_ID] FOREIGN KEY ([PK_Thesis_ID]) REFERENCES [dbo].[Thesis] ([ID])
GO