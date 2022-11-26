CREATE TABLE [dbo].[StudentClasses] (
  [ID] [nvarchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [nvarchar](200) NOT NULL,
  [StudentQuantity] [int] NOT NULL,
  [Notes] [nvarchar](200) NOT NULL,
  CONSTRAINT [PK_StudentClass_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO