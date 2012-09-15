/****** Object:  Table [dbo].[Posts]    Script Date: 08/31/2012 15:47:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Posts](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[UrlSlug] [nvarchar](100) NOT NULL,
	[IntroMarkdown] [nvarchar](max) NULL,
	[BodyMarkdown] [nvarchar](max) NULL,
	[IntroHtml] [nvarchar](max) NULL,
	[BodyHtml] [nvarchar](max) NULL,
	[IsPublished] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[PublishedOn] [datetime] NULL,
	[LastUpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Users]    Script Date: 08/31/2012 15:47:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](100) NOT NULL,
	[HashedPassword] [nvarchar](100) NOT NULL,
	[Salt] [nvarchar](100) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Posts] ADD  CONSTRAINT [DF_Posts_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
GO