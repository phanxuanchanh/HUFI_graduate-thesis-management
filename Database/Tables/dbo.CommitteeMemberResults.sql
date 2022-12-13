﻿CREATE TABLE [dbo].[CommitteeMemberResults] (
  [ThesisId] [varchar](50) NOT NULL,
  [CommitteeMemberId] [varchar](50) NOT NULL,
  [Point] [decimal] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  [EvaluationId] [varchar](50) NOT NULL,
  CONSTRAINT [PK_CouncilMembers_ID] PRIMARY KEY CLUSTERED ([ThesisId], [CommitteeMemberId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[CommitteeMemberResults]
  ADD CONSTRAINT [FK_CommitteeMemberResults_CommitteeMembers_ID] FOREIGN KEY ([CommitteeMemberId]) REFERENCES [dbo].[CommitteeMembers] ([ID])
GO

ALTER TABLE [dbo].[CommitteeMemberResults]
  ADD CONSTRAINT [FK_CommitteeMemberResults_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID])
GO