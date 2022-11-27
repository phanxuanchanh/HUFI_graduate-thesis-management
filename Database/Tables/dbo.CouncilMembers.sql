CREATE TABLE [dbo].[CouncilMembers] (
  [ID] [varchar](50) NOT NULL,
  [councilId] [varchar](50) NULL,
  CONSTRAINT [PK_CouncilMembers_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO