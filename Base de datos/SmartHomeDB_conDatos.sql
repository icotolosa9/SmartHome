USE [SmartHomeDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[Companies]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[Devices]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[HomeDevices]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[HomeOwnerPermissions]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[Homes]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[HomeUser]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[Notifications]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[Room]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[Sessions]    Script Date: 21/11/2024 8:59:39 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 21/11/2024 8:59:39 ******/
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
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240930130713_InitialMigration', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241001015148_ChangesInUser', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241002233102_UsersTypes', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241003150201_AddUserTypeDiscriminator', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241003151248_AddRoleDiscriminator', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241003151744_AddRoleDiscriminatorChanges', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241003191449_UpdateUserHierarchyRole', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241003195837_HomesAndCompanyInContext', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241003220239_RenameHome1ToHome', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241003222517_ChangedHomeInfo', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241004203804_Sessions', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241005170838_AddMemberPermissionsToHome', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241005185237_CompanyChangesInContext', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241006034929_Devices', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241007202253_AddHomeDevice', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241007230550_SeedAdminData', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241008191945_RemoveNameInHome', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241008193331_RemoveNameInHome2', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241020163420_CompanyIdInCompanyOwner', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241021213707_AddNotifications', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241028224528_NewDevicesTypes', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241031225639_UnifyRoles', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241031230354_AddHomeName', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241101215413_AddedRoomsToHomes', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241102212514_AddNameToHomeDevice', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241107160710_ModelValidatorInCompany', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241118172421_UpdateUserHomeRelationship', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241119175222_ModelValidatorStringInCompany', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241119180446_AllowNullModeloValidador', N'8.0.8')
GO
INSERT [dbo].[Companies] ([CompanyId], [Name], [OwnerId], [LogoURL], [Rut], [ValidatorModel]) VALUES (N'b3b7837c-d007-48db-a09b-15aab61e5601', N'Xiaomi', N'5d41c5e5-af9c-4f63-b943-b8b3030c3f64', N'fotoperfil.com', N'159995978231', N'ThreeLettersThreeNumbers')
INSERT [dbo].[Companies] ([CompanyId], [Name], [OwnerId], [LogoURL], [Rut], [ValidatorModel]) VALUES (N'd2a4fbf3-be9f-4166-8d65-c1a87b6a5198', N'GreenHome Inc', N'88bb5924-fb17-4ffd-88a9-5670f33e06c3', N'fotoprincipal.com', N'159876542025', N'OnlySixLetters')
INSERT [dbo].[Companies] ([CompanyId], [Name], [OwnerId], [LogoURL], [Rut], [ValidatorModel]) VALUES (N'66511fd4-cd62-42e6-9a3e-d17ff3553701', N'GoTech', N'5f6a0646-658e-4331-ac56-471fc24726a2', N'gotechouruguay.com/foto', N'456789552007', N'')
GO
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'476dab2c-a4e2-49d6-819d-0f40151b39d7', N'Sensor de ventana para exterior', N'SVI877', N'Eficiente y moderno', N'["fotosensor.com"]', 0, N'Window Sensor', N'b3b7837c-d007-48db-a09b-15aab61e5601', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'e41b75b8-da4e-4d26-84d6-1183bad343c0', N'Sensor movimiento exterior', N'MOVSNS', N'Última tecnología', N'["foto_sensores/movimiento"]', 0, N'Motion Sensor', N'd2a4fbf3-be9f-4166-8d65-c1a87b6a5198', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'95ed08d6-8570-4e8b-9547-3f16329ac053', N'Cámara Interior', N'CMS845', N'Cámara para interiores', N'["/fotos/camara_interior.jpg"]', 0, N'Camera', N'b3b7837c-d007-48db-a09b-15aab61e5601', 1, 0, 1, 0, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'adc7b221-0187-4180-8a5d-526f37370876', N'Sensor de Movimiento para Jardines', N'MOT450', N'Sensor ideal para áreas al aire libre', N'["sensor_mov_jardin.uy"]', 0, N'Motion Sensor', N'b3b7837c-d007-48db-a09b-15aab61e5601', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'59652dfc-587e-486e-8d87-544db68dc5db', N'Sensor de Movimiento Estándar', N'MSS305', N'Sensor para áreas interiores', N'["fotos.com/sensor_mov_estandar"]', 0, N'Motion Sensor', N'b3b7837c-d007-48db-a09b-15aab61e5601', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'8da3382e-13ce-4131-9a25-5b3fbef67399', N'Cámara Avanzada HD', N'CAM250', N'Cámara HD con detección de movimiento', N'["camara_hd.jpg"]', 0, N'Camera', N'b3b7837c-d007-48db-a09b-15aab61e5601', 1, 1, 1, 0, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'f7f9f61c-0b59-4b76-a89d-77164b3f73ea', N'Sensor de movimiento', N'SM334N', N'Última tecnología', N'["photo.com"]', 0, N'Motion Sensor', N'66511fd4-cd62-42e6-9a3e-d17ff3553701', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'f540004c-56db-4acf-a19b-799b673c57cd', N'Lámpara LED Inteligente', N'SLS400', N'Lámpara controlable vía app', N'["fotoLamp.com","photo.com","fotoLamp.com/exterior"]', 0, N'Smart Lamp', N'b3b7837c-d007-48db-a09b-15aab61e5601', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'9b72179d-8ab7-47c7-8967-a80b2588a77a', N'Lámpara RGB Inteligente', N'SLR900', N'Lámpara con colores personalizables', N'["photoLamp.com","smartLamp.com"]', 0, N'Smart Lamp', N'b3b7837c-d007-48db-a09b-15aab61e5601', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'94c508ff-879d-4318-bfbe-bf4823e76633', N'Cámara detectora de personas', N'CMPERS', N'Cámara con visión nocturna', N'["photo.com"]', 0, N'Camera', N'd2a4fbf3-be9f-4166-8d65-c1a87b6a5198', 1, 1, 0, 1, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'2d6a5031-4b56-46a0-9e8d-d0ce27ab6b9b', N'Sensor ventana ecológico', N'WINSNS', N'Eficiente y moderno', N'["foto.sensores.com"]', 0, N'Window Sensor', N'd2a4fbf3-be9f-4166-8d65-c1a87b6a5198', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'b5cde7a4-dceb-4aca-863c-d3ebb45cfa0f', N'Sensor de Movimiento Avanzado', N'MOVSENS-50', N'Sensor con sensibilidad ajustable', N'["sodimac.com/sensores"]', 0, N'Motion Sensor', N'66511fd4-cd62-42e6-9a3e-d17ff3553701', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'c8ba3001-05b4-46f7-9139-ea35fe60ebd0', N'Cámara de Seguridad Básica', N'CAM100', N'Cámara con visión nocturna', N'["fotos.camara_basica.jpg"]', 0, N'Camera', N'b3b7837c-d007-48db-a09b-15aab61e5601', 0, 1, 0, 1, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'89e32fc9-29a6-46e2-8812-f2a549e08f44', N'Cámara Movimiento Solar', N'CAMSOL', N'Detecta movimiento. Carga con un panel solar. ', N'["camaras/solar_mov"]', 0, N'Camera', N'd2a4fbf3-be9f-4166-8d65-c1a87b6a5198', 0, 1, 1, 0, NULL)
INSERT [dbo].[Devices] ([Id], [Name], [ModelNumber], [Description], [Photos], [IsOnline], [DeviceType], [CompanyId], [ForIndoorUse], [ForOutdoorUse], [SupportsMotionDetection], [SupportsPersonDetection], [IsOpen]) VALUES (N'3f0e020d-731a-4aba-b4da-fd9b13a216cc', N'Cámara Solar HD', N'SOLAR-CAM20', N'Cámara con panel solar para exteriores', N'["fotos.com/camara_solar"]', 0, N'Camera', N'66511fd4-cd62-42e6-9a3e-d17ff3553701', 0, 1, 1, 0, NULL)
GO
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'1a52f217-04b2-4991-9a8b-2256cc384640', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'8da3382e-13ce-4131-9a25-5b3fbef67399', 1, 0, N'efd83a63-0d3e-4895-d525-08dd09d8cf52', N'Cámara Avanzada HD')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'd6c8b1d8-a19f-458c-8e4f-304e1f5877a1', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'adc7b221-0187-4180-8a5d-526f37370876', 1, 0, N'e940f74c-c04f-49a3-d527-08dd09d8cf52', N'Sensor movimiento azotea')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'c125f10b-ee4e-4d64-a753-36543ebb396d', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'f540004c-56db-4acf-a19b-799b673c57cd', 0, 0, NULL, N'Lámpara LED Inteligente')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'94211265-4096-4dc7-aefb-5f56186cf5c5', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'adc7b221-0187-4180-8a5d-526f37370876', 0, 0, NULL, N'Sensor de Movimiento para Jardines')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'8e1a02f5-1faa-4a84-b753-771f4a73a1d5', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'c8ba3001-05b4-46f7-9139-ea35fe60ebd0', 1, 0, N'fede9568-f845-4caa-d526-08dd09d8cf52', N'Cámara de Seguridad Básica')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'a58aead5-c5ac-493f-9a4c-81d16c12be07', N'1c476be5-c634-44fc-924b-9b2ca2cc39e7', N'c8ba3001-05b4-46f7-9139-ea35fe60ebd0', 1, 0, NULL, N'Cámara de Seguridad Básica')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'9046b3ac-cb93-4f9d-ae16-860da073d32a', N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'c8ba3001-05b4-46f7-9139-ea35fe60ebd0', 1, 0, N'80abbb9b-4541-47d9-d523-08dd09d8cf52', N'Cámara de Seguridad Básica')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'4d05278a-d112-4fab-ad89-8d25315a077d', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'59652dfc-587e-486e-8d87-544db68dc5db', 1, 0, NULL, N'Sensor de Movimiento Estándar')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'6b4ff380-db05-4ed9-85ca-8e6c3078c5ab', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'adc7b221-0187-4180-8a5d-526f37370876', 1, 0, NULL, N'Sensor de Movimiento para Jardines')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'99e7ac3f-f796-4660-9403-ae6e49343c7f', N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'9b72179d-8ab7-47c7-8967-a80b2588a77a', 1, 1, NULL, N'Lámpara RGB Inteligente')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'c8a07d55-272a-48c3-818c-d7c7684151da', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'95ed08d6-8570-4e8b-9547-3f16329ac053', 1, 0, N'dcdf84c4-bc62-43dc-d524-08dd09d8cf52', N'Cámara personas cuarto')
INSERT [dbo].[HomeDevices] ([HardwareId], [HomeId], [DeviceId], [Connected], [IsOpenOrOn], [RoomId], [Name]) VALUES (N'ede28f19-23b6-423e-b8de-eaefad07be3c', N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'8da3382e-13ce-4131-9a25-5b3fbef67399', 0, 0, NULL, N'Cámara Avanzada HD')
GO
INSERT [dbo].[HomeOwnerPermissions] ([Id], [HomeId], [HomeOwnerId], [Permission], [IsNotificationEnabled]) VALUES (N'bfbcb838-291b-4b0c-e052-08dd09d8a8fe', N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'6e2b5c0f-e3e0-496c-a4aa-08d73a096268', N'listDevices, AssignDevice, RenameDevice, AddDevice', 1)
INSERT [dbo].[HomeOwnerPermissions] ([Id], [HomeId], [HomeOwnerId], [Permission], [IsNotificationEnabled]) VALUES (N'477a5db8-b6e7-40f0-e053-08dd09d8a8fe', N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'7f0aba35-b874-4581-b775-591ebf08b46d', N'listDevices, AddDevice', 1)
INSERT [dbo].[HomeOwnerPermissions] ([Id], [HomeId], [HomeOwnerId], [Permission], [IsNotificationEnabled]) VALUES (N'27a391d1-2863-4487-e054-08dd09d8a8fe', N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'6df4e2d1-9442-4466-adbc-ea9fe703644f', N'', 0)
INSERT [dbo].[HomeOwnerPermissions] ([Id], [HomeId], [HomeOwnerId], [Permission], [IsNotificationEnabled]) VALUES (N'5ac815d1-b78a-4279-e055-08dd09d8a8fe', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'listDevices, AssignDevice, RenameDevice, AddDevice', 1)
INSERT [dbo].[HomeOwnerPermissions] ([Id], [HomeId], [HomeOwnerId], [Permission], [IsNotificationEnabled]) VALUES (N'cbd9d04c-32a8-406f-e056-08dd09d8a8fe', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'6df4e2d1-9442-4466-adbc-ea9fe703644f', N'listDevices, RenameDevice, AddDevice', 1)
INSERT [dbo].[HomeOwnerPermissions] ([Id], [HomeId], [HomeOwnerId], [Permission], [IsNotificationEnabled]) VALUES (N'4135e37e-6c3b-4b30-e057-08dd09d8a8fe', N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'1209493b-45d7-44f2-a5dc-4c0b9acbf52f', N'listDevices', 0)
INSERT [dbo].[HomeOwnerPermissions] ([Id], [HomeId], [HomeOwnerId], [Permission], [IsNotificationEnabled]) VALUES (N'bd08e70b-553a-41e8-e058-08dd09d8a8fe', N'1c476be5-c634-44fc-924b-9b2ca2cc39e7', N'6e2b5c0f-e3e0-496c-a4aa-08d73a096268', N'listDevices, AssignDevice, RenameDevice, AddDevice', 1)
GO
INSERT [dbo].[Homes] ([Id], [HomeOwnerId], [Address], [Capacity], [Location], [Name]) VALUES (N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'7f0aba35-b874-4581-b775-591ebf08b46d', N'19 de abril 1558', 5, N'(-30.47, -49.56)', N'Casitas del prado')
INSERT [dbo].[Homes] ([Id], [HomeOwnerId], [Address], [Capacity], [Location], [Name]) VALUES (N'4546167e-4ebf-46af-927a-379ac42569f6', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'Talcahuano 3395', 4, N'(-33.57, -54.688)', N'Casa Mateo')
INSERT [dbo].[Homes] ([Id], [HomeOwnerId], [Address], [Capacity], [Location], [Name]) VALUES (N'c024fc3f-5272-4493-87f8-790a5a29aebb', N'7f0aba35-b874-4581-b775-591ebf08b46d', N'Aconcagua 9', 4, N'(-32.4, -51.3)', N'Bella vista')
INSERT [dbo].[Homes] ([Id], [HomeOwnerId], [Address], [Capacity], [Location], [Name]) VALUES (N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'e0151575-3825-447a-b4c8-8b8c62dc59b9', N'Obligado 1123', 4, N'(-33.29, -52.47)', N'Aguas calmas')
INSERT [dbo].[Homes] ([Id], [HomeOwnerId], [Address], [Capacity], [Location], [Name]) VALUES (N'1c476be5-c634-44fc-924b-9b2ca2cc39e7', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'Acuario 44', 6, N'(-34.9, -54.56)', N'Cabaña en la playa')
INSERT [dbo].[Homes] ([Id], [HomeOwnerId], [Address], [Capacity], [Location], [Name]) VALUES (N'e7e9d3b5-c4fa-410a-8786-9e8cf920e08c', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'Jackson 1319', 3, N'(-33.15, -56.17)', N'Apartamento en Cordón')
GO
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'6e2b5c0f-e3e0-496c-a4aa-08d73a096268')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'1c476be5-c634-44fc-924b-9b2ca2cc39e7', N'6e2b5c0f-e3e0-496c-a4aa-08d73a096268')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'1209493b-45d7-44f2-a5dc-4c0b9acbf52f')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'7f0aba35-b874-4581-b775-591ebf08b46d')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'c024fc3f-5272-4493-87f8-790a5a29aebb', N'7f0aba35-b874-4581-b775-591ebf08b46d')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'7f0aba35-b874-4581-b775-591ebf08b46d')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'e0151575-3825-447a-b4c8-8b8c62dc59b9')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'4546167e-4ebf-46af-927a-379ac42569f6', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'1c476be5-c634-44fc-924b-9b2ca2cc39e7', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'e7e9d3b5-c4fa-410a-8786-9e8cf920e08c', N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'68d228c9-0e2e-4757-9e18-1fca5c09c263', N'6df4e2d1-9442-4466-adbc-ea9fe703644f')
INSERT [dbo].[HomeUser] ([HomeId], [UserId]) VALUES (N'c258a51d-c73c-4c17-8edd-7f518d701ec8', N'6df4e2d1-9442-4466-adbc-ea9fe703644f')
GO
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'd187625c-5fd7-4bf3-9000-02f3b9ef516a', N'Se detectó a la persona con el mail: jgon8@smarthome.com.', CAST(N'2024-11-21T00:17:41.3509044' AS DateTime2), 0, N'6df4e2d1-9442-4466-adbc-ea9fe703644f', N'8e1a02f5-1faa-4a84-b753-771f4a73a1d5', N'Camera')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'9b00ab9f-c36a-4ec5-bc23-35984af427bb', N'Se detectó una persona desconocida.', CAST(N'2024-11-21T00:18:03.2348857' AS DateTime2), 0, N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'8e1a02f5-1faa-4a84-b753-771f4a73a1d5', N'Camera')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'f21e5b94-aaf2-4442-aeb3-3ece6d77ed5e', N'Se detectó una persona desconocida.', CAST(N'2024-11-21T00:18:03.2510144' AS DateTime2), 0, N'7f0aba35-b874-4581-b775-591ebf08b46d', N'8e1a02f5-1faa-4a84-b753-771f4a73a1d5', N'Motion Sensor')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'f85164e7-148c-4e50-bd30-48a1998020ce', N'Se detectó una persona desconocida.', CAST(N'2024-11-21T00:18:03.2484611' AS DateTime2), 0, N'6df4e2d1-9442-4466-adbc-ea9fe703644f', N'8e1a02f5-1faa-4a84-b753-771f4a73a1d5', N'Camera')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'c23de336-a9a8-4ab7-8531-75f0f4935bd4', N'Se detectó movimiento.', CAST(N'2024-11-21T00:16:44.2323708' AS DateTime2), 0, N'7f0aba35-b874-4581-b775-591ebf08b46d', N'c8a07d55-272a-48c3-818c-d7c7684151da', N'Motion Sensor')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'6c4f831c-2328-4915-a5c8-90dab635772d', N'Se detectó movimiento.', CAST(N'2024-11-21T00:15:15.8654031' AS DateTime2), 0, N'7f0aba35-b874-4581-b775-591ebf08b46d', N'1a52f217-04b2-4991-9a8b-2256cc384640', N'Motion Sensor')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'ef682a18-25aa-4a3c-85e8-a14653144207', N'Se prendió la lámpara.', CAST(N'2024-11-21T00:04:18.6484782' AS DateTime2), 0, N'6e2b5c0f-e3e0-496c-a4aa-08d73a096268', N'99e7ac3f-f796-4660-9403-ae6e49343c7f', N'Smart Lamp')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'0e5b2307-9224-4200-8016-ab3963a92dbf', N'Se prendió la lámpara.', CAST(N'2024-11-21T00:04:18.6745077' AS DateTime2), 0, N'e0151575-3825-447a-b4c8-8b8c62dc59b9', N'99e7ac3f-f796-4660-9403-ae6e49343c7f', N'Motion Sensor')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'b8d2761c-8581-4ea3-9437-b01a39376f5e', N'Se prendió la lámpara.', CAST(N'2024-11-21T00:04:18.6704088' AS DateTime2), 1, N'7f0aba35-b874-4581-b775-591ebf08b46d', N'99e7ac3f-f796-4660-9403-ae6e49343c7f', N'Smart Lamp')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'181a2e26-a7fd-4bfe-b7b3-bbf4b6db332c', N'Se detectó a la persona con el mail: jgon8@smarthome.com.', CAST(N'2024-11-21T00:17:41.3368824' AS DateTime2), 0, N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'8e1a02f5-1faa-4a84-b753-771f4a73a1d5', N'Camera')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'd21674e8-244a-47b7-8a6d-d734b29dce9b', N'Se detectó movimiento.', CAST(N'2024-11-21T00:15:15.8493184' AS DateTime2), 0, N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'1a52f217-04b2-4991-9a8b-2256cc384640', N'Camera')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'6e40f758-82ff-4977-a17c-e4e7586af981', N'Se detectó movimiento.', CAST(N'2024-11-21T00:16:44.2282888' AS DateTime2), 0, N'6df4e2d1-9442-4466-adbc-ea9fe703644f', N'c8a07d55-272a-48c3-818c-d7c7684151da', N'Camera')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'923a61aa-c7c6-4f80-a442-e5b898dc9063', N'Se detectó movimiento.', CAST(N'2024-11-21T00:16:44.2129184' AS DateTime2), 0, N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'c8a07d55-272a-48c3-818c-d7c7684151da', N'Camera')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'83a68c5f-74ea-4f83-b046-ea0ff414588f', N'Se detectó a la persona con el mail: jgon8@smarthome.com.', CAST(N'2024-11-21T00:17:41.3536230' AS DateTime2), 0, N'7f0aba35-b874-4581-b775-591ebf08b46d', N'8e1a02f5-1faa-4a84-b753-771f4a73a1d5', N'Motion Sensor')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'756b92b4-9965-4389-bc7c-f524c608d307', N'Se detectó movimiento.', CAST(N'2024-11-21T00:13:59.8496673' AS DateTime2), 1, N'7f0aba35-b874-4581-b775-591ebf08b46d', N'd6c8b1d8-a19f-458c-8e4f-304e1f5877a1', N'Motion Sensor')
INSERT [dbo].[Notifications] ([Id], [Event], [Date], [IsRead], [UserId], [HardwareId], [DeviceType]) VALUES (N'de3bd3ee-9f72-445f-b5c2-f68c6a938b0c', N'Se detectó movimiento.', CAST(N'2024-11-21T00:13:59.8328118' AS DateTime2), 0, N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'd6c8b1d8-a19f-458c-8e4f-304e1f5877a1', N'Motion Sensor')
GO
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'e48dba81-6abb-4576-d520-08dd09d8cf52', N'Living', N'c258a51d-c73c-4c17-8edd-7f518d701ec8')
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'23139d0f-3e36-41b4-d521-08dd09d8cf52', N'Comedor', N'c258a51d-c73c-4c17-8edd-7f518d701ec8')
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'9d35abda-2c5b-4aea-d522-08dd09d8cf52', N'Cocina', N'c258a51d-c73c-4c17-8edd-7f518d701ec8')
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'80abbb9b-4541-47d9-d523-08dd09d8cf52', N'Jardín', N'c258a51d-c73c-4c17-8edd-7f518d701ec8')
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'dcdf84c4-bc62-43dc-d524-08dd09d8cf52', N'Cuarto principal', N'68d228c9-0e2e-4757-9e18-1fca5c09c263')
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'efd83a63-0d3e-4895-d525-08dd09d8cf52', N'Cocina', N'68d228c9-0e2e-4757-9e18-1fca5c09c263')
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'fede9568-f845-4caa-d526-08dd09d8cf52', N'Barbacoa', N'68d228c9-0e2e-4757-9e18-1fca5c09c263')
INSERT [dbo].[Room] ([Id], [Name], [HomeId]) VALUES (N'e940f74c-c04f-49a3-d527-08dd09d8cf52', N'Azotea', N'68d228c9-0e2e-4757-9e18-1fca5c09c263')
GO
SET IDENTITY_INSERT [dbo].[Sessions] ON 

INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (1, N'9bb1e88d-c613-460a-8530-dcbdc1484f91', N'admin@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (2, N'dfa16702-3c34-48cb-bac1-5815296ade18', N'admin@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (3, N'f5ef2ba7-24f8-47c7-9e52-bf1241081f8a', N'verofajo@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (4, N'aec3c9db-0f96-49fc-8fdc-95e2fc4a5532', N'mrod99@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (5, N'd103982b-5686-4dec-97ee-bf1a33f50402', N'ftolo@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (6, N'5205fb93-cf23-4f2f-a07b-05b9e741abad', N'mrod99@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (7, N'065d196e-10ee-4d30-b666-96b38c215b67', N'admin@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (8, N'36db733a-625e-4ae6-bd2a-7b870a429284', N'ftolo@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (9, N'd3061b66-c96d-4147-8247-9bb85099ce1a', N'admin@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (10, N'49e95622-ae48-4afd-a5fd-fb026f884ff9', N'luchorod@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (11, N'54c8fadb-4c74-4372-897a-9b798cc09503', N'verofajo@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (12, N'3837613c-1d0a-4099-b599-498066da0cf9', N'dieguitoF@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (13, N'109c0074-c9d0-4319-b095-4f49d6455556', N'juan@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (14, N'cc41d8b9-6250-465c-ae63-3cdcb39f246b', N'verofajo@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (15, N'db23bdd7-07b4-434e-874a-04a66f095b96', N'verofajo@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (16, N'ecce221b-2629-4cdb-830d-0a2e86e62ec6', N'admin@smarthome.com')
INSERT [dbo].[Sessions] ([Id], [Token], [UserEmail]) VALUES (17, N'dd15a467-e0f2-40b3-9ad1-d7fec81bb47d', N'tongas@smarthome.com')
SET IDENTITY_INSERT [dbo].[Sessions] OFF
GO
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'6e2b5c0f-e3e0-496c-a4aa-08d73a096268', N'Martina', N'Gómez', N'martugo@smarthome.com', N'Password123!', N'homeOwner', N'myProfilePhoto.com', CAST(N'2024-11-20T23:57:59.3350042' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'5f6a0646-658e-4331-ac56-471fc24726a2', N'Mariana', N'Rodríguez', N'mrod99@smarthome.com', N'Password123!', N'companyOwner', NULL, CAST(N'2024-11-20T20:27:13.4037287' AS DateTime2), N'66511fd4-cd62-42e6-9a3e-d17ff3553701')
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'1209493b-45d7-44f2-a5dc-4c0b9acbf52f', N'Diego', N'Forlán', N'dieguitoF@smarthome.com', N'Password123!', N'homeOwner', N'fotodeperfil.com', CAST(N'2024-11-20T20:28:35.4351080' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'eb8e6559-6376-4fba-a51f-54eaf5ed6301', N'Luis', N'Mejía', N'lmejia@smarthome.com', N'Password123!', N'admin', NULL, CAST(N'2024-11-20T20:24:45.2653509' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'88bb5924-fb17-4ffd-88a9-5670f33e06c3', N'Gastón', N'González', N'tongas@smarthome.com', N'Password123!', N'companyOwner', NULL, CAST(N'2024-11-21T00:21:37.6586937' AS DateTime2), N'd2a4fbf3-be9f-4166-8d65-c1a87b6a5198')
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'7f0aba35-b874-4581-b775-591ebf08b46d', N'Verónica', N'Fajo', N'verofajo@smarthome.com', N'Password123!', N'homeOwner', N'photo.com', CAST(N'2024-11-20T20:27:38.4657573' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'e0151575-3825-447a-b4c8-8b8c62dc59b9', N'Luciano', N'Rodríguez', N'luchorod@smarthome.com', N'Password123!', N'homeOwner', N'profilePhoto.com', CAST(N'2024-11-20T20:28:12.9590767' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'e2fbfb1f-8571-45d7-9230-8eb2ebb92bf3', N'Admin', N'User', N'admin@smarthome.com', N'Password123!', N'admin', N'photo.com', CAST(N'2024-10-29T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'5d41c5e5-af9c-4f63-b943-b8b3030c3f64', N'Francisco', N'Tolosa', N'ftolo@smarthome.com', N'Password123!', N'companyOwner', NULL, CAST(N'2024-11-20T20:25:14.2740414' AS DateTime2), N'b3b7837c-d007-48db-a09b-15aab61e5601')
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'6df4e2d1-9442-4466-adbc-ea9fe703644f', N'Juan', N'Rodríguez', N'juan@smarthome.com', N'Password123!', N'admin', NULL, CAST(N'2024-11-20T20:23:44.4161224' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [Password], [Role], [ProfilePhoto], [CreationDate], [CompanyId]) VALUES (N'7ebf7edb-5e58-4753-88bb-f696432d7546', N'Juana', N'González', N'jgon8@smarthome.com', N'Password123!', N'homeOwner', N'otrafoto.com', CAST(N'2024-11-20T20:28:59.6298155' AS DateTime2), NULL)
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
