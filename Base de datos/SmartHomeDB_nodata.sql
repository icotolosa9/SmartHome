USE [master]
GO
/****** Object:  Database [SmartHomeDB]    Script Date: 20/11/2024 20:17:47 ******/
CREATE DATABASE [SmartHomeDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SmartHomeDB', FILENAME = N'C:\Users\jottones.MVDNTB52655\SmartHomeDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SmartHomeDB_log', FILENAME = N'C:\Users\jottones.MVDNTB52655\SmartHomeDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SmartHomeDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SmartHomeDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SmartHomeDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SmartHomeDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SmartHomeDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SmartHomeDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SmartHomeDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [SmartHomeDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SmartHomeDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SmartHomeDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SmartHomeDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SmartHomeDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SmartHomeDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SmartHomeDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SmartHomeDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SmartHomeDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SmartHomeDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SmartHomeDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SmartHomeDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SmartHomeDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SmartHomeDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SmartHomeDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SmartHomeDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [SmartHomeDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SmartHomeDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SmartHomeDB] SET  MULTI_USER 
GO
ALTER DATABASE [SmartHomeDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SmartHomeDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SmartHomeDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SmartHomeDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SmartHomeDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SmartHomeDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SmartHomeDB] SET QUERY_STORE = OFF
GO
USE [SmartHomeDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 20/11/2024 20:17:48 ******/
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
/****** Object:  Table [dbo].[Companies]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[CompanyId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[LogoURL] [nvarchar](max) NOT NULL,
	[Rut] [nvarchar](max) NOT NULL,
	[ValidatorModel] [nvarchar](max) NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ModelNumber] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Photos] [nvarchar](max) NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[DeviceType] [nvarchar](13) NOT NULL,
	[CompanyId] [uniqueidentifier] NOT NULL,
	[ForIndoorUse] [bit] NULL,
	[ForOutdoorUse] [bit] NULL,
	[SupportsMotionDetection] [bit] NULL,
	[SupportsPersonDetection] [bit] NULL,
	[IsOpen] [bit] NULL,
 CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HomeDevices]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeDevices](
	[HardwareId] [uniqueidentifier] NOT NULL,
	[HomeId] [uniqueidentifier] NOT NULL,
	[DeviceId] [uniqueidentifier] NOT NULL,
	[Connected] [bit] NOT NULL,
	[IsOpenOrOn] [bit] NOT NULL,
	[RoomId] [uniqueidentifier] NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_HomeDevices] PRIMARY KEY CLUSTERED 
(
	[HardwareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HomeOwnerPermissions]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeOwnerPermissions](
	[Id] [uniqueidentifier] NOT NULL,
	[HomeId] [uniqueidentifier] NOT NULL,
	[HomeOwnerId] [uniqueidentifier] NOT NULL,
	[Permission] [nvarchar](max) NOT NULL,
	[IsNotificationEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_HomeOwnerPermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Homes]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Homes](
	[Id] [uniqueidentifier] NOT NULL,
	[HomeOwnerId] [uniqueidentifier] NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Capacity] [int] NOT NULL,
	[Location] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Homes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HomeUser]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeUser](
	[HomeId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_HomeUser] PRIMARY KEY CLUSTERED 
(
	[HomeId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [uniqueidentifier] NOT NULL,
	[Event] [nvarchar](max) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[HardwareId] [uniqueidentifier] NOT NULL,
	[DeviceType] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[HomeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [uniqueidentifier] NOT NULL,
	[UserEmail] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20/11/2024 20:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](13) NOT NULL,
	[ProfilePhoto] [nvarchar](max) NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[CompanyId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Devices_CompanyId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_Devices_CompanyId] ON [dbo].[Devices]
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HomeDevices_DeviceId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_HomeDevices_DeviceId] ON [dbo].[HomeDevices]
(
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HomeDevices_HomeId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_HomeDevices_HomeId] ON [dbo].[HomeDevices]
(
	[HomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HomeDevices_RoomId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_HomeDevices_RoomId] ON [dbo].[HomeDevices]
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HomeOwnerPermissions_HomeId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_HomeOwnerPermissions_HomeId] ON [dbo].[HomeOwnerPermissions]
(
	[HomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HomeOwnerPermissions_HomeOwnerId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_HomeOwnerPermissions_HomeOwnerId] ON [dbo].[HomeOwnerPermissions]
(
	[HomeOwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Homes_HomeOwnerId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_Homes_HomeOwnerId] ON [dbo].[Homes]
(
	[HomeOwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_HomeUser_UserId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_HomeUser_UserId] ON [dbo].[HomeUser]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_UserId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_UserId] ON [dbo].[Notifications]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Room_HomeId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE NONCLUSTERED INDEX [IX_Room_HomeId] ON [dbo].[Room]
(
	[HomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_CompanyId]    Script Date: 20/11/2024 20:17:48 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_CompanyId] ON [dbo].[Users]
(
	[CompanyId] ASC
)
WHERE ([CompanyId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (N'') FOR [LogoURL]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (N'') FOR [Rut]
GO
ALTER TABLE [dbo].[HomeDevices] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsOpenOrOn]
GO
ALTER TABLE [dbo].[HomeDevices] ADD  DEFAULT (N'') FOR [Name]
GO
ALTER TABLE [dbo].[HomeOwnerPermissions] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsNotificationEnabled]
GO
ALTER TABLE [dbo].[Homes] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [HomeOwnerId]
GO
ALTER TABLE [dbo].[Homes] ADD  DEFAULT (N'') FOR [Address]
GO
ALTER TABLE [dbo].[Homes] ADD  DEFAULT ((0)) FOR [Capacity]
GO
ALTER TABLE [dbo].[Homes] ADD  DEFAULT (N'') FOR [Location]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreationDate]
GO
ALTER TABLE [dbo].[Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Devices] CHECK CONSTRAINT [FK_Devices_Companies_CompanyId]
GO
ALTER TABLE [dbo].[HomeDevices]  WITH CHECK ADD  CONSTRAINT [FK_HomeDevices_Devices_DeviceId] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Devices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HomeDevices] CHECK CONSTRAINT [FK_HomeDevices_Devices_DeviceId]
GO
ALTER TABLE [dbo].[HomeDevices]  WITH CHECK ADD  CONSTRAINT [FK_HomeDevices_Homes_HomeId] FOREIGN KEY([HomeId])
REFERENCES [dbo].[Homes] ([Id])
GO
ALTER TABLE [dbo].[HomeDevices] CHECK CONSTRAINT [FK_HomeDevices_Homes_HomeId]
GO
ALTER TABLE [dbo].[HomeDevices]  WITH CHECK ADD  CONSTRAINT [FK_HomeDevices_Room_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HomeDevices] CHECK CONSTRAINT [FK_HomeDevices_Room_RoomId]
GO
ALTER TABLE [dbo].[HomeOwnerPermissions]  WITH CHECK ADD  CONSTRAINT [FK_HomeOwnerPermissions_Homes_HomeId] FOREIGN KEY([HomeId])
REFERENCES [dbo].[Homes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HomeOwnerPermissions] CHECK CONSTRAINT [FK_HomeOwnerPermissions_Homes_HomeId]
GO
ALTER TABLE [dbo].[Homes]  WITH CHECK ADD  CONSTRAINT [FK_Homes_Users_HomeOwnerId] FOREIGN KEY([HomeOwnerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Homes] CHECK CONSTRAINT [FK_Homes_Users_HomeOwnerId]
GO
ALTER TABLE [dbo].[HomeUser]  WITH CHECK ADD  CONSTRAINT [FK_HomeUser_Homes_HomeId] FOREIGN KEY([HomeId])
REFERENCES [dbo].[Homes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HomeUser] CHECK CONSTRAINT [FK_HomeUser_Homes_HomeId]
GO
ALTER TABLE [dbo].[HomeUser]  WITH CHECK ADD  CONSTRAINT [FK_HomeUser_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HomeUser] CHECK CONSTRAINT [FK_HomeUser_Users_UserId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Users_UserId]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_Homes_HomeId] FOREIGN KEY([HomeId])
REFERENCES [dbo].[Homes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_Homes_HomeId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Companies_CompanyId]
GO
USE [master]
GO
ALTER DATABASE [SmartHomeDB] SET  READ_WRITE 
GO
