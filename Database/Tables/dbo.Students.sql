CREATE TABLE [dbo].[Students] (
  [ID] [varchar](50) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Phone] [varchar](20) NOT NULL,
  [Email] [varchar](200) NOT NULL,
  [Adress] [nvarchar](400) NULL,
  [Birthday] [date] NULL,
  CONSTRAINT [PK_Students_ID] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO