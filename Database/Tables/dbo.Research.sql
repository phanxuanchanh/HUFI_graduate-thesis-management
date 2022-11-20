CREATE TABLE [dbo].[Research] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](50) NOT NULL,
  CONSTRAINT [PK_Research_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO