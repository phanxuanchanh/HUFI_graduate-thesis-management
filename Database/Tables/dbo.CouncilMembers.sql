CREATE TABLE [dbo].[CouncilMembers] (
  [ID] [varchar](50) NOT NULL,
  [CouncilId] [varchar](50) NOT NULL,
  [FacultyStaffId] [varchar](50) NULL,
  [IsChairman] [bit] NULL,
  [IsSecretary] [bit] NULL,
  [IsCouncillor] [bit] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_CouncilMembers_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[CouncilMembers]
  ADD CONSTRAINT [FK_CouncilMembers_Councils_ID] FOREIGN KEY ([CouncilId]) REFERENCES [dbo].[Councils] ([ID])
GO

ALTER TABLE [dbo].[CouncilMembers]
  ADD CONSTRAINT [FK_CouncilMembers_FacultyStaffs_ID] FOREIGN KEY ([FacultyStaffId]) REFERENCES [dbo].[FacultyStaffs] ([ID])
GO