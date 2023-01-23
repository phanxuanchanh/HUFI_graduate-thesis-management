CREATE TABLE [dbo].[ThesisGroupDetails] (
  [StudentThesisGroupId] [varchar](50) NOT NULL,
  [StudentId] [varchar](50) NOT NULL,
  [Notes] [ntext] NULL,
  [IsCompleted] [bit] NOT NULL,
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

ALTER TABLE [dbo].[ThesisGroupDetails]
  ADD CONSTRAINT [FK_ThesisGroupDetails_Students_ID] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ThesisGroupDetails]
  ADD CONSTRAINT [FK_ThesisGroupDetails_ThesisGroups_ID] FOREIGN KEY ([StudentThesisGroupId]) REFERENCES [dbo].[ThesisGroups] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO