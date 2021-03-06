USE [MailDB]
GO
/****** Object:  Table [dbo].[Inbox]    Script Date: 25.02.2018 23:33:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inbox](
	[LetterId] [uniqueidentifier] NOT NULL,
	[AddresseeId] [uniqueidentifier] NOT NULL,
	[AddresserId] [uniqueidentifier] NOT NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_Inbox] PRIMARY KEY CLUSTERED 
(
	[LetterId] ASC,
	[AddresseeId] ASC,
	[AddresserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Letters]    Script Date: 25.02.2018 23:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Letters](
	[Id] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Letters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SentMail]    Script Date: 25.02.2018 23:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SentMail](
	[LetterId] [uniqueidentifier] NOT NULL,
	[AddresserId] [uniqueidentifier] NOT NULL,
	[AddresseeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SentMail_1] PRIMARY KEY CLUSTERED 
(
	[LetterId] ASC,
	[AddresserId] ASC,
	[AddresseeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 25.02.2018 23:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Inbox]  WITH CHECK ADD  CONSTRAINT [FK_Inbox_Letters] FOREIGN KEY([LetterId])
REFERENCES [dbo].[Letters] ([Id])
GO
ALTER TABLE [dbo].[Inbox] CHECK CONSTRAINT [FK_Inbox_Letters]
GO
ALTER TABLE [dbo].[Inbox]  WITH CHECK ADD  CONSTRAINT [FK_Inbox_Users] FOREIGN KEY([AddresseeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Inbox] CHECK CONSTRAINT [FK_Inbox_Users]
GO
ALTER TABLE [dbo].[Inbox]  WITH CHECK ADD  CONSTRAINT [FK_Inbox_Users1] FOREIGN KEY([AddresserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Inbox] CHECK CONSTRAINT [FK_Inbox_Users1]
GO
ALTER TABLE [dbo].[SentMail]  WITH CHECK ADD  CONSTRAINT [FK_SentMail_Letters] FOREIGN KEY([LetterId])
REFERENCES [dbo].[Letters] ([Id])
GO
ALTER TABLE [dbo].[SentMail] CHECK CONSTRAINT [FK_SentMail_Letters]
GO
ALTER TABLE [dbo].[SentMail]  WITH CHECK ADD  CONSTRAINT [FK_SentMail_Users] FOREIGN KEY([AddresserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SentMail] CHECK CONSTRAINT [FK_SentMail_Users]
GO
ALTER TABLE [dbo].[SentMail]  WITH CHECK ADD  CONSTRAINT [FK_SentMail_Users1] FOREIGN KEY([AddresseeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SentMail] CHECK CONSTRAINT [FK_SentMail_Users1]
GO
