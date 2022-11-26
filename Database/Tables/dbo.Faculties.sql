﻿CREATE TABLE [dbo].[Faculties] (
  [ID] [varchar](50) NOT NULL,
  [Name] [varchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [Dean] [nvarchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  CONSTRAINT [PK_Faculty_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO