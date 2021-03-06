USE [WebApiDb]
GO
/****** Object:  Table [dbo].[User]    Script Date: 05/07/2015 12:13:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([UserId], [UserName], [Password], [Name]) VALUES (1, N'Admin', N'Admin', N'Admin User')
INSERT [dbo].[User] ([UserId], [UserName], [Password], [Name]) VALUES (2, N'Developer', N'Developer', N'Developer User')
SET IDENTITY_INSERT [dbo].[User] OFF
/****** Object:  Table [dbo].[Tokens]    Script Date: 05/07/2015 12:13:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tokens](
	[TokenId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[AuthToken] [nvarchar](250) NOT NULL,
	[IssuedOn] [datetime] NOT NULL,
	[ExpiresOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[TokenId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Tokens_User]    Script Date: 05/07/2015 12:13:46 ******/
ALTER TABLE [dbo].[Tokens]  WITH CHECK ADD  CONSTRAINT [FK_Tokens_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Tokens] CHECK CONSTRAINT [FK_Tokens_User]
GO

DROP Table [Services]
/****** Object:  Table [dbo].[Services]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ServiceId][int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar] (20) NOT NULL,
	[ServiceVersion] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Services] ON
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [ServiceVersion]) VALUES (1, N'Service 1', N'1.1.1')
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [ServiceVersion]) VALUES (2, N'ABC', N'1.1.1')
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [ServiceVersion]) VALUES (3, N'Service 3', N'1.1.2')
SET IDENTITY_INSERT [dbo].[Services] OFF
GO

/****** Object:  Table [dbo].[FeatureToggle]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeatureToggle](
	[Name] [nvarchar] (50) NOT NULL,
	[Value] [bit] NOT NULL
 CONSTRAINT [PK_FeatureToggle] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[FeatureToggle] ([Name], [Value]) VALUES (N'isButtonBlue', 1)
INSERT [dbo].[FeatureToggle] ([Name], [Value]) VALUES (N'isButtonGreen', 1)
INSERT [dbo].[FeatureToggle] ([Name], [Value]) VALUES (N'isButtonRed', 1)
GO


/****** Object:  Table [dbo].[ServiceFeatureToggle]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].ServiceFeatureToggle(
	[ServiceId] [int] REFERENCES [dbo].[Services]([ServiceId]) NOT NULL,
	[FeatureToggleName][nvarchar] (50) REFERENCES [dbo].[FeatureToggle]([Name]) NOT NULL,
	[CustomValue] [bit] NULL
 CONSTRAINT [PK_ServiceFeatureToggle] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC,
	[FeatureToggleName] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[ServiceFeatureToggle] ([ServiceId], [FeatureToggleName], [CustomValue]) VALUES (1, N'isButtonBlue',NULL)
INSERT [dbo].[ServiceFeatureToggle] ([ServiceId], [FeatureToggleName], [CustomValue]) VALUES (2, N'isButtonBlue',1)
INSERT [dbo].[ServiceFeatureToggle] ([ServiceId], [FeatureToggleName], [CustomValue]) VALUES (3, N'isButtonBlue',NULL)
INSERT [dbo].[ServiceFeatureToggle] ([ServiceId], [FeatureToggleName], [CustomValue]) VALUES (2, N'isButtonGreen',NULL)
INSERT [dbo].[ServiceFeatureToggle] ([ServiceId], [FeatureToggleName], [CustomValue]) VALUES (1, N'isButtonRed',NULL)
INSERT [dbo].[ServiceFeatureToggle] ([ServiceId], [FeatureToggleName], [CustomValue]) VALUES (3, N'isButtonRed',NULL)

