CREATE TABLE [dbo].[Council] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  [Chairman] [nvarchar](50) NOT NULL,
  [CouncilPoint] [int] NOT NULL,
  CONSTRAINT [PK_Council_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO