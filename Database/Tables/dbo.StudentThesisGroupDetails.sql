CREATE TABLE [dbo].[StudentThesisGroupDetails] (
  [StudentThesisGroupId] [varchar](50) NOT NULL,
  [StudentId] [varchar](50) NOT NULL,
  [Notes] [ntext] NULL,
  [IsApproved] [bit] NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_StudentThesisGroupDetail_PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([StudentThesisGroupId], [StudentId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentThesisGroupDetails]
  ADD CONSTRAINT [FK_StudentThesisGroupDetails_Students_ID] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([ID])
GO

ALTER TABLE [dbo].[StudentThesisGroupDetails]
  ADD CONSTRAINT [FK_StudentThesisGroupDetails_StudentThesisGroups_ID] FOREIGN KEY ([StudentThesisGroupId]) REFERENCES [dbo].[StudentThesisGroups] ([ID])
GO