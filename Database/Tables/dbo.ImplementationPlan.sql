CREATE TABLE [dbo].[ImplementationPlan] (
  [ID] [varchar](50) NOT NULL,
  [ThesisId] [varchar](50) NOT NULL,
  [From] [datetime] NOT NULL,
  [To] [datetime] NOT NULL,
  [Task] [ntext] NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NULL,
  CONSTRAINT [PK_ImplementationPlan_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ImplementationPlan]
  ADD CONSTRAINT [FK_ImplementationPlan_Theses_ID] FOREIGN KEY ([ThesisId]) REFERENCES [dbo].[Theses] ([ID])
GO