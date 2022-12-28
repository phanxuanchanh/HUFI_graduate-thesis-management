CREATE TABLE [dbo].[FacultyStaffs] (
  [ID] [varchar](50) NOT NULL,
  [FacultyId] [varchar](50) NOT NULL,
  [FacultyRoleId] [varchar](50) NOT NULL,
  [FullName] [nvarchar](100) NOT NULL,
  [Description] [ntext] NULL,
  [Gender] [nvarchar](10) NOT NULL,
  [Phone] [varchar](20) NOT NULL,
  [Address] [nvarchar](50) NOT NULL,
  [Email] [nvarchar](50) NOT NULL,
  [Birthday] [date] NOT NULL,
  [Avatar] [nvarchar](50) NULL,
  [Position] [nvarchar](50) NULL,
  [Degree] [nvarchar](50) NOT NULL,
  [Notes] [nvarchar](200) NULL,
  [Password] [nvarchar](100) NOT NULL,
  [Salt] [nvarchar](100) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_FacultyStaff_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[FacultyStaffs]
  ADD CONSTRAINT [FK_FacultyStaffs_AppRoles_ID ] FOREIGN KEY ([FacultyRoleId]) REFERENCES [dbo].[AppRoles] ([ID ])
GO

ALTER TABLE [dbo].[FacultyStaffs]
  ADD CONSTRAINT [FK_FacultyStaffs_Faculties_ID] FOREIGN KEY ([FacultyId]) REFERENCES [dbo].[Faculties] ([ID])
GO