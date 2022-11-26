﻿CREATE TABLE [dbo].[StudentThesisGroups] (
  [ID] [varchar](50) NOT NULL,
  [ThesisId] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  [StudentQuantity] [int] NOT NULL,
  [Notes] [ntext] NULL,
  CONSTRAINT [PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO