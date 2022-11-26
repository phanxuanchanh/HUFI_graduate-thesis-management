CREATE TABLE [dbo].[Topics] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NOT NULL,
  [Notes] [ntext] NOT NULL,
  CONSTRAINT [PK_Topic_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO