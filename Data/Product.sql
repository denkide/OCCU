USE [MVCApp]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 5/7/2020 8:43:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[QuantityOnHand] [int] NOT NULL,
	[BackorderFlag] [bit] NOT NULL,
	[DateLastUpdate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_BackorderFlag]  DEFAULT ((0)) FOR [BackorderFlag]
GO

ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_DateLastUpdate]  DEFAULT (getdate()) FOR [DateLastUpdate]
GO


