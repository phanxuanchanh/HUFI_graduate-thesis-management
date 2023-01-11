CREATE TABLE [dbo].[AppUserRoles] (
  [UserId] [varchar](50) NOT NULL,
  [RoleId] [varchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  CONSTRAINT [PK_AppUserRoles_UserId] PRIMARY KEY CLUSTERED ([UserId], [RoleId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[AppUserRoles]
  ADD CONSTRAINT [FK_AppUserRoles_AppRoles_ID ] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AppRoles] ([ID ])
GO

ALTER TABLE [dbo].[AppUserRoles]
  ADD CONSTRAINT [FK_AppUserRoles_FacultyStaffs_ID] FOREIGN KEY ([UserId]) REFERENCES [dbo].[FacultyStaffs] ([ID])
GO