CREATE TABLE [dbo].[Council] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Comment] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  [Chairman] [nvarchar](50) NOT NULL,
  [CouncilPoint] [float] NOT NULL,
  [Numberofmember] [int] NOT NULL,
  CONSTRAINT [PK_Council_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO