CREATE TABLE [dbo].[StudentClasses] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Description] [nvarchar](200) NULL,
  [StudentQuantity] [int] NOT NULL,
  [Notes] [nvarchar](200) NULL,
  [FacultyId] [varchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  CONSTRAINT [PK_StudentClass_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentClasses]
  ADD CONSTRAINT [FK_StudentClasses_Faculties_ID] FOREIGN KEY ([FacultyId]) REFERENCES [dbo].[Faculties] ([ID])
GO