CREATE TABLE [dbo].[AppUserRoles] (
  [UserId] [varchar](50) NOT NULL,
  [RoleId] [varchar](50) NOT NULL,
  [Description] [ntext] NULL,
  [Notes] [ntext] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  CONSTRAINT [PK_AppUserRoles_UserId] PRIMARY KEY CLUSTERED ([UserId], [RoleId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AppUserRoles]
  ADD CONSTRAINT [FK_AppUserRoles_AppRoles_ID ] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AppRoles] ([ID ]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[AppUserRoles]
  ADD CONSTRAINT [FK_AppUserRoles_Faculties_ID] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Faculties] ([ID]) ON DELETE CASCADE
GO