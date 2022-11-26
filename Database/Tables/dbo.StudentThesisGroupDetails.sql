﻿CREATE TABLE [dbo].[StudentThesisGroupDetails] (
  [StudentThesisGroupId] [varchar](50) NOT NULL,
  [StudentId] [varchar](50) NOT NULL,
  [Notes] [ntext] NULL,
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