CREATE TABLE [dbo].[StudentThesisGroup] (
  [ID] [varchar](50) NOT NULL,
  [PK_Students_ID] [varchar](50) NOT NULL,
  [PK_Thesis_ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](50) NOT NULL,
  [StudentQuantity] [nvarchar](50) NOT NULL,
  [Notes] [bigint] NOT NULL,
  CONSTRAINT [PK_StudentThesisGroup_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO