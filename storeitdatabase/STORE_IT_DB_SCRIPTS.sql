CREATE DATABASE [STOREIT_DB]
GO
USE [STOREIT_DB]
GO
/****** Object:  Schema [files]    Script Date: 23/06/2025 10:55:04 PM ******/
CREATE SCHEMA [files]
GO
/****** Object:  Schema [identity]    Script Date: 23/06/2025 10:55:04 PM ******/
CREATE SCHEMA [identity]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OwnersFiles]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OwnersFiles](
	[UserId] [varchar](36) NOT NULL,
	[FileId] [varchar](36) NOT NULL,
 CONSTRAINT [PK_OwnersFiles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserFileDto]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFileDto](
	[FileId] [varchar](36) NOT NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[Url] [nvarchar](256) NOT NULL,
	[TypeId] [varchar](36) NOT NULL,
	[FileTypeName] [nvarchar](50) NOT NULL,
	[ExtensionId] [varchar](36) NOT NULL,
	[FileExtensionName] [nvarchar](50) NOT NULL,
	[Size] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[OwnerFirstName] [nvarchar](15) NOT NULL,
	[OwnerId] [varchar](36) NOT NULL,
	[OwnerImageUrl] [nvarchar](256) NULL,
	[OwnerLastName] [nvarchar](15) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserFileSharedDto]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFileSharedDto](
	[Id] [varchar](36) NULL,
	[FirstName] [nvarchar](15) NOT NULL,
	[LastName] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[ImageUrl] [nvarchar](256) NULL,
	[CreatedAt] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersFiles]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersFiles](
	[UserId] [varchar](36) NOT NULL,
	[FileId] [varchar](36) NOT NULL,
 CONSTRAINT [PK_UsersFiles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [files].[FileExtensions]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [files].[FileExtensions](
	[Id] [varchar](36) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FileExtensions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [files].[Files]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [files].[Files](
	[Id] [varchar](36) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Url] [nvarchar](256) NOT NULL,
	[TypeId] [varchar](36) NOT NULL,
	[ExtensionId] [varchar](36) NOT NULL,
	[Size] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [files].[FileTypes]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [files].[FileTypes](
	[Id] [varchar](36) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FileTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [identity].[Roles]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [identity].[Roles](
	[Id] [varchar](36) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[NormalizedName] [nvarchar](50) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [identity].[RolesClaims]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [identity].[RolesClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [varchar](36) NOT NULL,
	[ClaimType] [nvarchar](50) NOT NULL,
	[ClaimValue] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_RolesClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [identity].[Users]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [identity].[Users](
	[Id] [varchar](36) NOT NULL,
	[FirstName] [nvarchar](15) NOT NULL,
	[LastName] [nvarchar](15) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ImageUrl] [nvarchar](256) NULL,
	[Email] [nvarchar](50) NOT NULL,
	[NormalizedEmail] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[NormalizedUserName] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [identity].[UsersClaims]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [identity].[UsersClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](36) NOT NULL,
	[ClaimType] [nvarchar](50) NOT NULL,
	[ClaimValue] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UsersClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [identity].[UsersLogins]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [identity].[UsersLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](100) NOT NULL,
	[UserId] [varchar](36) NOT NULL,
 CONSTRAINT [PK_UsersLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [identity].[UsersRoles]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [identity].[UsersRoles](
	[UserId] [varchar](36) NOT NULL,
	[RoleId] [varchar](36) NOT NULL,
 CONSTRAINT [PK_UsersRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [identity].[UsersTokens]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [identity].[UsersTokens](
	[UserId] [varchar](36) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_UsersTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserFileDto] ADD  CONSTRAINT [DF__UserFileD__Owner__19AACF41]  DEFAULT (N'') FOR [OwnerFirstName]
GO
ALTER TABLE [dbo].[UserFileDto] ADD  CONSTRAINT [DF__UserFileD__Owner__1A9EF37A]  DEFAULT ('') FOR [OwnerId]
GO
ALTER TABLE [dbo].[UserFileDto] ADD  CONSTRAINT [DF__UserFileD__Owner__1C873BEC]  DEFAULT (N'') FOR [OwnerLastName]
GO
ALTER TABLE [files].[FileExtensions] ADD  DEFAULT (upper(CONVERT([varchar](36),newid()))) FOR [Id]
GO
ALTER TABLE [files].[Files] ADD  DEFAULT (upper(CONVERT([varchar](36),newid()))) FOR [Id]
GO
ALTER TABLE [files].[FileTypes] ADD  DEFAULT (upper(CONVERT([varchar](36),newid()))) FOR [Id]
GO
ALTER TABLE [identity].[Roles] ADD  DEFAULT (upper(CONVERT([varchar](36),newid()))) FOR [Id]
GO
ALTER TABLE [identity].[Users] ADD  DEFAULT (upper(CONVERT([varchar](36),newid()))) FOR [Id]
GO
ALTER TABLE [identity].[Users] ADD  DEFAULT (N'') FOR [NormalizedUserName]
GO
ALTER TABLE [identity].[Users] ADD  DEFAULT (N'') FOR [UserName]
GO
ALTER TABLE [dbo].[OwnersFiles]  WITH CHECK ADD  CONSTRAINT [FK_OwnersFiles_Files_FileId] FOREIGN KEY([FileId])
REFERENCES [files].[Files] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OwnersFiles] CHECK CONSTRAINT [FK_OwnersFiles_Files_FileId]
GO
ALTER TABLE [dbo].[OwnersFiles]  WITH CHECK ADD  CONSTRAINT [FK_OwnersFiles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [identity].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OwnersFiles] CHECK CONSTRAINT [FK_OwnersFiles_Users_UserId]
GO
ALTER TABLE [dbo].[UsersFiles]  WITH CHECK ADD  CONSTRAINT [FK_UsersFiles_Files_FileId] FOREIGN KEY([FileId])
REFERENCES [files].[Files] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsersFiles] CHECK CONSTRAINT [FK_UsersFiles_Files_FileId]
GO
ALTER TABLE [dbo].[UsersFiles]  WITH CHECK ADD  CONSTRAINT [FK_UsersFiles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [identity].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsersFiles] CHECK CONSTRAINT [FK_UsersFiles_Users_UserId]
GO
ALTER TABLE [files].[Files]  WITH CHECK ADD  CONSTRAINT [FK_Files_FileExtensions_ExtensionId] FOREIGN KEY([ExtensionId])
REFERENCES [files].[FileExtensions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [files].[Files] CHECK CONSTRAINT [FK_Files_FileExtensions_ExtensionId]
GO
ALTER TABLE [files].[Files]  WITH CHECK ADD  CONSTRAINT [FK_Files_FileTypes_TypeId] FOREIGN KEY([TypeId])
REFERENCES [files].[FileTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [files].[Files] CHECK CONSTRAINT [FK_Files_FileTypes_TypeId]
GO
ALTER TABLE [identity].[RolesClaims]  WITH CHECK ADD  CONSTRAINT [FK_RolesClaims_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [identity].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [identity].[RolesClaims] CHECK CONSTRAINT [FK_RolesClaims_Roles_RoleId]
GO
ALTER TABLE [identity].[UsersClaims]  WITH CHECK ADD  CONSTRAINT [FK_UsersClaims_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [identity].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [identity].[UsersClaims] CHECK CONSTRAINT [FK_UsersClaims_Users_UserId]
GO
ALTER TABLE [identity].[UsersLogins]  WITH CHECK ADD  CONSTRAINT [FK_UsersLogins_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [identity].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [identity].[UsersLogins] CHECK CONSTRAINT [FK_UsersLogins_Users_UserId]
GO
ALTER TABLE [identity].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [identity].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [identity].[UsersRoles] CHECK CONSTRAINT [FK_UsersRoles_Roles_RoleId]
GO
ALTER TABLE [identity].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [identity].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [identity].[UsersRoles] CHECK CONSTRAINT [FK_UsersRoles_Users_UserId]
GO
ALTER TABLE [identity].[UsersTokens]  WITH CHECK ADD  CONSTRAINT [FK_UsersTokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [identity].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [identity].[UsersTokens] CHECK CONSTRAINT [FK_UsersTokens_Users_UserId]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserFiles]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetUserFiles]
    @UserId varchar(36)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        uf.FileId, 
        f.Name as FileName, 
        f.Url, 
        f.TypeId, 
        t.Name as FileTypeName, 
        f.ExtensionId, 
        e.Name as FileExtensionName, 
        f.Size, 
        f.CreatedAt,
		u.Id as OwnerId,
		u.FirstName as OwnerFirstName,
		u.LastName as OwnerLastName,
		u.ImageUrl as OwnerImageUrl
    FROM dbo.UsersFiles uf
    INNER JOIN files.Files f ON uf.FileId = f.Id
	INNER JOIN dbo.OwnersFiles owf ON owf.FileId = f.Id
    INNER JOIN [identity].Users u ON u.Id = owf.UserId
	INNER JOIN files.FileTypes t ON f.TypeId = t.Id 
    INNER JOIN files.FileExtensions e ON f.ExtensionId = e.Id 
    WHERE uf.UserId = @UserId
	ORDER BY f.CreatedAt DESC
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUsersForFileByFileId]    Script Date: 23/06/2025 10:55:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_GetUsersForFileByFileId]
@FileId varchar(36)
as
begin
select Id, FirstName, LastName, CreatedAt, ImageUrl, Email from [identity].Users u inner join dbo.UsersFiles uf on u.Id = uf.UserId where uf.FileId = @FileId
end
GO
