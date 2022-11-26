CREATE TABLE [dbo].[StudentThesisGroups] (
  [ID] [varchar](50) NOT NULL,
  [ThesisId] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  [StudentQuantity] [int] NOT NULL,
  [Notes] [ntext] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  CONSTRAINT [PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentThesisGroups]
  ADD CONSTRAINT [FK_StudentThesisGroups_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID])
GO