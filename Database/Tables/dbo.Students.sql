CREATE TABLE [dbo].[Students] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Phone] [varchar](20) NULL,
  [Email] [varchar](200) NOT NULL,
  [Address] [nvarchar](400) NULL,
  [Birthday] [date] NULL,
  [Avatar] [varchar](200) NULL,
  [Description] [ntext] NULL,
  [StudentClassId] [varchar](50) NOT NULL,
  [Password] [nvarchar](100) NOT NULL,
  [Salt] [nvarchar](100) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  [IsDeleted] [bit] NOT NULL,
  [VerificationCode] [varchar](100) NULL,
  [CodeExpTime] [datetime] NULL,
  [Gender] [nvarchar](10) NULL DEFAULT (N'Nam'),
  CONSTRAINT [PK_Students_ID] PRIMARY KEY CLUSTERED ([ID]),
  UNIQUE ([Phone]),
  UNIQUE ([Email])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Students]
  ADD CONSTRAINT [FK_Students_StudentClasses_ID] FOREIGN KEY ([StudentClassId]) REFERENCES [dbo].[StudentClasses] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
GO