CREATE TABLE [dbo].[CommitteeMembers] (
  [ID] [varchar](50) NOT NULL,
  [ThesisCommitteeId] [varchar](50) NOT NULL,
  [MemberId] [varchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  [Titles] [varchar](50) NOT NULL,
  CONSTRAINT [PK_CommitteeMembers_ThesisCommitteeId] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[CommitteeMembers]
  ADD CONSTRAINT [FK_CommitteeMembers_FacultyStaffs_ID] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[FacultyStaffs] ([ID])
GO

ALTER TABLE [dbo].[CommitteeMembers]
  ADD CONSTRAINT [FK_CommitteeMembers_ThesisCommittees_ID] FOREIGN KEY ([ThesisCommitteeId]) REFERENCES [dbo].[ThesisCommittees] ([ID])
GO