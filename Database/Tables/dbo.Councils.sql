CREATE TABLE [dbo].[Councils] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Description] [ntext] NULL,
  [Semester] [int] NULL,
  [Year] [nvarchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_Councils_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO