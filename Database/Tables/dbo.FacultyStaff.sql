CREATE TABLE [dbo].[FacultyStaff] (
  [ID] [varchar](50) NOT NULL,
  [PK_Faculty_ID] [varchar](50) NOT NULL,
  [Description] [nvarchar](200) NOT NULL,
  [Quantity] [int] NOT NULL,
  CONSTRAINT [PK_FacultyStaff_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[FacultyStaff]
  ADD CONSTRAINT [FK_FacultyStaff_Faculty_ID] FOREIGN KEY ([PK_Faculty_ID]) REFERENCES [dbo].[Faculty] ([ID])
GO