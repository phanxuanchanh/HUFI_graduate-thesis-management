CREATE TABLE [dbo].[CommitteeMembers] (
  [ID] [varchar](50) NOT NULL,
  [ThesisId] [varchar](50) NOT NULL,
  [CouncilId] [varchar](50) NOT NULL,
  [FacultyStaffId] [varchar](50) NULL,
  [Titles] [varchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_CouncilMembers_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[CommitteeMembers]
  ADD CONSTRAINT [FK_CommitteeMembers_ThesisCommittees_ID] FOREIGN KEY ([CouncilId]) REFERENCES [dbo].[ThesisCommittees] ([ID])
GO

ALTER TABLE [dbo].[CommitteeMembers]
  ADD CONSTRAINT [FK_CouncilMembers_FacultyStaffs_ID] FOREIGN KEY ([FacultyStaffId]) REFERENCES [dbo].[FacultyStaffs] ([ID])
GO