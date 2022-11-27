CREATE TABLE [dbo].[Councils] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NOT NULL,
  [Notes] [ntext] NOT NULL,
  [Chairman] [nvarchar](50) NOT NULL,
  [CouncilPoint] [numeric] NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  CONSTRAINT [PK_Council_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO