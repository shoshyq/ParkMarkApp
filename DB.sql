USE [master]
GO
/****** Object:  Database [ParkMarkDB]    Script Date: 19/07/2021 19:20:34 ******/
CREATE DATABASE [ParkMarkDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ParkMarkDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ParkMarkDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ParkMarkDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ParkMarkDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ParkMarkDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ParkMarkDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ParkMarkDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ParkMarkDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ParkMarkDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ParkMarkDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ParkMarkDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ParkMarkDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ParkMarkDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ParkMarkDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ParkMarkDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ParkMarkDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ParkMarkDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ParkMarkDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ParkMarkDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ParkMarkDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ParkMarkDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ParkMarkDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ParkMarkDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ParkMarkDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ParkMarkDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ParkMarkDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ParkMarkDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ParkMarkDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ParkMarkDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ParkMarkDB] SET  MULTI_USER 
GO
ALTER DATABASE [ParkMarkDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ParkMarkDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ParkMarkDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ParkMarkDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ParkMarkDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ParkMarkDB] SET QUERY_STORE = OFF
GO
USE [ParkMarkDB]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[CityName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedbacks]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedbacks](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[DescriberUserCode] [int] NULL,
	[DescriptedUserCode] [int] NULL,
	[Feedback] [varchar](500) NULL,
	[Rating] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParkingSpot]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParkingSpot](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[UserCode] [int] NOT NULL,
	[CityCode] [int] NULL,
	[Place_id] [varchar](1000) NULL,
	[FullAddress] [varchar](120) NULL,
	[SpotWidth] [float] NULL,
	[SpotLength] [float] NULL,
	[PricePerHour] [float] NULL,
	[HasRoof] [bit] NULL,
	[DaysSchedule] [int] NULL,
	[IsOccupied] [bit] NULL,
	[AvRegularly] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParkingSpotSearch]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParkingSpotSearch](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[MyLocationAddress] [varchar](120) NULL,
	[Place_id] [varchar](1000) NULL,
	[CityCode] [int] NULL,
	[SizeOpt] [bit] NULL,
	[PreferableWidth] [float] NULL,
	[PreferableLength] [float] NULL,
	[RoofOpt] [bit] NULL,
	[DaysSchedule] [int] NULL,
	[MinPrice] [float] NULL,
	[MaxPrice] [float] NULL,
	[Regularly] [bit] NULL,
	[SearchDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentDetails]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentDetails](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[CreditCardNumber] [varchar](16) NULL,
	[ExpiryDateMonth] [char](2) NULL,
	[ExpiryDateYear] [char](2) NULL,
	[SecurityCode] [varchar](4) NULL,
	[PostalCode] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchResults]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchResults](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Usercode] [int] NULL,
	[SearchCode] [int] NULL,
	[ResultPSCode] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](30) NULL,
	[UserPassword] [varchar](50) NULL,
	[UserEmail] [varchar](100) NULL,
	[UserPhoneNumber] [varchar](10) NULL,
	[PaymentDetails1] [int] NULL,
	[PaymentDetails2] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WeekDays]    Script Date: 19/07/2021 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeekDays](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Sunday] [varchar](1000) NULL,
	[Monday] [varchar](1000) NULL,
	[Tuesday] [varchar](1000) NULL,
	[Wednesday] [varchar](1000) NULL,
	[Thursday] [varchar](1000) NULL,
	[Friday] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cities] ON 

INSERT [dbo].[Cities] ([Code], [CityName]) VALUES (1, N'Kiryat Ata')
INSERT [dbo].[Cities] ([Code], [CityName]) VALUES (2, N'Jerusalem')
INSERT [dbo].[Cities] ([Code], [CityName]) VALUES (3, N'Rekhasim')
INSERT [dbo].[Cities] ([Code], [CityName]) VALUES (4, N'Haifa')
SET IDENTITY_INSERT [dbo].[Cities] OFF
GO
SET IDENTITY_INSERT [dbo].[Feedbacks] ON 

INSERT [dbo].[Feedbacks] ([Code], [DescriberUserCode], [DescriptedUserCode], [Feedback], [Rating]) VALUES (1, 1, 2, N'it was great! thank you', 5)
SET IDENTITY_INSERT [dbo].[Feedbacks] OFF
GO
SET IDENTITY_INSERT [dbo].[ParkingSpot] ON 

INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (1, 2, 3, N'EiBIYVNoYWtlZCBTdCAxMiwgUmVraGFzaW0sIElzcmFlbCIwEi4KFAoSCTk9sJG9sR0VEem7dgOJO1ztEAwqFAoSCSdORpC9sR0VEYIX33xfMW7I', N'Hashaked 12, Rekhasim', 0, NULL, 6, 1, 32, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (2, 3, 3, N'ChIJjayglqKxHRURv_Q4Iy3-GfM', N'Savyon St 46, Rekhasim', 0, NULL, 7, 1, 33, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (3, 12, 3, N'ChIJ4UnKo72xHRUR0fPR5Si4klA', N'Khatsav St 23, Rekhasim', 0, NULL, 5.5, 1, 34, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (4, 2, 3, N'ChIJx6f-NJaxHRUR5uV1DDv06j0', N'Khatsav St 3, Rekhasim', 0, NULL, 8, 1, 35, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (5, 4, 3, N'Eh9LaGF0c2F2IFN0IDE3LCBSZWtoYXNpbSwgSXNyYWVsIjASLgoUChIJuUvQh72xHRURaPEjfAl9-pQQESoUChIJlWGUh72xHRURbWuj6uK5BuI', N'Khatsav St 17, Rekhasim', 0, NULL, 6, 1, 36, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (6, 5, 3, N'EiBIYVJhdiBLdWsgU3QgNSwgUmVraGFzaW0sIElzcmFlbCIwEi4KFAoSCQ8Q9cC-sR0VEXcU7zxJnkzWEAUqFAoSCfsUdua-sR0VEXVT7QmLHzS4', N'HaRav Kuk 5, Rekhasim', 0, NULL, 7.2, 1, 37, 0, 0)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (7, 6, 3, N'EiBIYVJha2Fmb3QgU3QgNSwgUmVraGFzaW0sIElzcmFlbCIwEi4KFAoSCcMPXOS8sR0VEfU3swITF6mcEAUqFAoSCb_8Pba8sR0VEU8mwY9GP298', N'HaRakafot 5, Rekhasim', 0, NULL, 6.5, 1, 38, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (8, 12, 3, N'EiBIYVNoYWtlZCBTdCAyNCwgUmVraGFzaW0sIElzcmFlbCIwEi4KFAoSCTk9sJG9sR0VEem7dgOJO1ztEBgqFAoSCSdORpC9sR0VEYIX33xfMW7I', N'Hashaked 24, Rekhasim', NULL, NULL, 5, 0, 37, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (9, 9, 3, N'EiBIYVNoYWtlZCBTdCAxNSwgUmVraGFzaW0sIElzcmFlbCIwEi4KFAoSCTk9sJG9sR0VEei7dgOJO1ztEA8qFAoSCSdORpC9sR0VEYIX33xfMW7I', N'Hashaked 15, Rekhasim', 0, NULL, NULL, 1, 32, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (10, 10, 3, N'EhtLaGFydXYgU3QsIFJla2hhc2ltLCBJc3JhZWwiLiosChQKEglVF12SorEdFRErUznhTvZT_BIUChIJjf2Fs76xHRURx_ZC6qZv19c', N'Haruv Street 7, Rekhasim', 0, 2, 5, 1, 33, 0, 1)
INSERT [dbo].[ParkingSpot] ([Code], [UserCode], [CityCode], [Place_id], [FullAddress], [SpotWidth], [SpotLength], [PricePerHour], [HasRoof], [DaysSchedule], [IsOccupied], [AvRegularly]) VALUES (11, 11, 3, N'Eh1TZSdvcmEgU3QgMywgUmVraGFzaW0sIElzcmFlbCIwEi4KFAoSCZ1bPk6jsR0VERugU-tY2ChCEAMqFAoSCc-V8E2jsR0VEXY9A9mwPjG8', N'Seora Street 3, Rekhasim, Israel', 0, NULL, NULL, 1, 34, 0, 1)
SET IDENTITY_INSERT [dbo].[ParkingSpot] OFF
GO
SET IDENTITY_INSERT [dbo].[ParkingSpotSearch] ON 

INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (104, 13, N'השקד 52, Rekhasim', N'EiPXlNep16fXkyA1Miwg16jXm9eh15nXnSwg15nXqdeo15DXnCIaEhgKFAoSCTk9sJG9sR0VEem7dgOJO1ztEDQ', 3, NULL, NULL, NULL, NULL, 143, 0, 10, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (105, 13, NULL, N'EiBIYVNoYWtlZCBTdCAxMCwgUmVraGFzaW0sIElzcmFlbCIaEhgKFAoSCTk9sJG9sR0VEem7dgOJO1ztEAo', 3, NULL, NULL, NULL, NULL, 144, 2, 8.5, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (106, 13, NULL, N'ChIJoUzKeSNKHRUR1levuZ_ojzM', 3, NULL, NULL, NULL, NULL, 145, 4, 9.5, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (107, 13, NULL, N'ChIJoUzKeSNKHRUR1levuZ_ojzM', 3, NULL, NULL, NULL, NULL, 146, 4, 9.5, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (108, 13, NULL, N'ChIJoUzKeSNKHRUR1levuZ_ojzM', 3, NULL, NULL, NULL, NULL, 147, 2, 8.5, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (109, 13, NULL, N'ChIJoUzKeSNKHRUR1levuZ_ojzM', 3, NULL, NULL, NULL, NULL, 148, 4, 9.5, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (110, 13, NULL, N'EiBIYVNoYWtlZCBTdCAxMCwgUmVraGFzaW0sIElzcmFlbCIaEhgKFAoSCTk9sJG9sR0VEem7dgOJO1ztEAo', 3, NULL, NULL, NULL, NULL, 149, 4, 10, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (111, 1, NULL, N'ChIJoUzKeSNKHRUR1levuZ_ojzM', 3, NULL, NULL, NULL, NULL, 150, 3.5, 10, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (112, 1, NULL, N'ChIJoUzKeSNKHRUR1levuZ_ojzM', 3, NULL, NULL, NULL, NULL, 151, 2, 10, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (113, 1, NULL, N'EiBIYVNoYWtlZCBTdCAxMCwgUmVraGFzaW0sIElzcmFlbCIaEhgKFAoSCTk9sJG9sR0VEem7dgOJO1ztEAo', 3, NULL, NULL, NULL, NULL, 152, 4.5, 10, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (114, 1, NULL, N'ChIJoUzKeSNKHRUR1levuZ_ojzM', 3, NULL, NULL, NULL, NULL, 153, 2, 10, 0, NULL)
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (119, 1, N'Hashaked 5, Rekhasim', N'EiLXlNep16fXkyA1LCDXqNeb16HXmdedLCDXmdep16jXkNecIhoSGAoUChIJFWPt25exHRURAHgrqW0cjwYQBQ', 3, 0, NULL, NULL, 0, 158, 2, 10, 1, CAST(N'2021-07-16T09:55:42.053' AS DateTime))
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (120, 7, N'העצמאות 15, Kiryat Ata', N'Ei7XlNei16bXnteQ15XXqiAxNSwg16fXqNeZ16og15DXqteQLCDXmdep16jXkNecIhoSGAoUChIJYfqGozWxHRUR4mLoUrvMomYQDw', 1, 0, NULL, NULL, 0, 161, 2, 10, 1, CAST(N'2021-07-16T10:09:29.410' AS DateTime))
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (121, 2, N'החרוב 2, Rekhasim', N'ChIJVRddkqKxHRURK1M54U72U_w', 3, 0, NULL, NULL, 0, 162, 2, 9.5, 1, CAST(N'2021-07-16T10:11:45.950' AS DateTime))
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (122, 4, N'Savyon 1, Rekhasim', N'EiTXodeR15nXldefIDEsINeo15vXodeZ150sINeZ16nXqNeQ15wiGhIYChQKEgk3sTAJmbEdFRHtxPEYr8l1wBAB', 3, 0, NULL, NULL, 0, 163, 0, 10, 1, CAST(N'2021-07-16T10:13:37.850' AS DateTime))
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (123, 5, N'לילך 3, Rekhasim', N'EiLXnNeZ15zXmiAzLCDXqNeb16HXmdedLCDXmdep16jXkNecIhoSGAoUChIJoQP0E5ixHRURoE3x_8mJOKMQAw', 3, 0, NULL, NULL, 0, 164, 0, 10, 1, CAST(N'2021-07-16T10:15:20.583' AS DateTime))
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (137, 1, N'השקד 52, Rekhasim', N'EiPXlNep16fXkyA1Miwg16jXm9eh15nXnSwg15nXqdeo15DXnCIaEhgKFAoSCTk9sJG9sR0VEem7dgOJO1ztEDQ', 3, 0, NULL, NULL, 0, 167, 2, 10, 1, CAST(N'2021-07-18T20:40:53.303' AS DateTime))
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (138, 1, N'Hashaked 5, Rekhasim', N'EiLXlNep16fXkyA1LCDXqNeb16HXmdedLCDXmdep16jXkNecIhoSGAoUChIJFWPt25exHRURAHgrqW0cjwYQBQ', 3, 0, NULL, NULL, 0, 168, 0, 10, 1, CAST(N'2021-07-18T20:59:37.857' AS DateTime))
INSERT [dbo].[ParkingSpotSearch] ([Code], [UserId], [MyLocationAddress], [Place_id], [CityCode], [SizeOpt], [PreferableWidth], [PreferableLength], [RoofOpt], [DaysSchedule], [MinPrice], [MaxPrice], [Regularly], [SearchDate]) VALUES (158, 1, N'השקד 52, Rekhasim', N'EiPXlNep16fXkyA1Miwg16jXm9eh15nXnSwg15nXqdeo15DXnCIaEhgKFAoSCTk9sJG9sR0VEem7dgOJO1ztEDQ', 3, NULL, NULL, NULL, NULL, 180, 2, 8.5, 1, CAST(N'2021-07-19T15:51:59.167' AS DateTime))
SET IDENTITY_INSERT [dbo].[ParkingSpotSearch] OFF
GO
SET IDENTITY_INSERT [dbo].[SearchResults] ON 

INSERT [dbo].[SearchResults] ([Code], [Usercode], [SearchCode], [ResultPSCode]) VALUES (11, 1, 137, 1)
INSERT [dbo].[SearchResults] ([Code], [Usercode], [SearchCode], [ResultPSCode]) VALUES (12, 2, 121, 2)
INSERT [dbo].[SearchResults] ([Code], [Usercode], [SearchCode], [ResultPSCode]) VALUES (13, 1, 158, 3)
INSERT [dbo].[SearchResults] ([Code], [Usercode], [SearchCode], [ResultPSCode]) VALUES (14, 1, 138, 4)
INSERT [dbo].[SearchResults] ([Code], [Usercode], [SearchCode], [ResultPSCode]) VALUES (15, 4, 122, 5)
INSERT [dbo].[SearchResults] ([Code], [Usercode], [SearchCode], [ResultPSCode]) VALUES (16, 1, 119, 7)
INSERT [dbo].[SearchResults] ([Code], [Usercode], [SearchCode], [ResultPSCode]) VALUES (17, 5, 123, 8)
SET IDENTITY_INSERT [dbo].[SearchResults] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (1, N'shanamish', N'mish123', N'mishmish@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (2, N'odaya', N'12345', N'odaya@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (3, N'elacohen', N'ela123', N'elacohen@gmail.com', N'0527493838', NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (4, N'tamar', N'tamar123', N'tamar123@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (5, N'suripinter', N'suri123', N'suripinter@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (6, N'simcha_cohen', N'simcha123', N'simchacohen@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (7, N'leahkatz', N'leah123', N'leahkatz@gmail.com', N'0538595876', NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (8, N'miriglassman', N'miri13', N'miriglass@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (9, N'chaya levin', N'chaya123', N'chayalevin@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (10, N'elina', N'elina123', N'elina@gmail.com', NULL, NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (11, N'dinabaram', N'dina123', N'dinal@gmail.com', N'0537897997', NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (12, N'shoshyustinov', N'shoshy123', N'shoshana.ustinov43770@gmail.com', N'0583249977', NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (13, N'chanakatz', N'chana123', N'tamid.lehajeh@gmail.com', N'0583249977', NULL, NULL)
INSERT [dbo].[Users] ([Code], [Username], [UserPassword], [UserEmail], [UserPhoneNumber], [PaymentDetails1], [PaymentDetails2]) VALUES (14, N'lovely_user', N'user123', N'shoshy.ustinov43770@gmail.com', N'0583249977', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[WeekDays] ON 

INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (8, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (11, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (15, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (17, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (18, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (20, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (24, NULL, NULL, NULL, NULL, N'08.00-13.45, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (25, N'20.00-00.00, ', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (32, N'08.00-13.00, 15.45-21.00, ', N'08.00-13.00, 15.45-21.00, ', NULL, NULL, N'09.00-18.00, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (33, N'10.00-15.30, ', NULL, N'09.00-16.00, ', N'09.00-16.00, ', N'09.00-13.00, 16.30-21.30', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (34, N'09.00-14.15, ', N'08.00-13.00, 15.45-21.00, ', NULL, NULL, N'12.00-18.00, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (35, NULL, N'08.00-17.30, ', N'08.00-17.30, ', N'08.00-17.30, ', N'08.00-17.30, ', N'08.00-12.00, ')
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (36, NULL, NULL, N'08.00-13.00, 15.45-21.00, ', NULL, N'09.00-18.0, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (37, NULL, N'08.00-13.00, ', NULL, N'15.00-21.00, 11.30-13.00, 08.00-10.00, ', N'18.30-22.00, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (38, NULL, N'08.00-13.00, 16.30-18.45, ', NULL, N'08.00-13.00, 15.45-21.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (131, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (132, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (133, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (134, NULL, NULL, NULL, N'09.00-11.00, 09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (135, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (136, NULL, NULL, NULL, N'09.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (137, NULL, NULL, NULL, N'09.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (138, NULL, NULL, NULL, N'10.59-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (139, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (140, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (141, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (142, NULL, NULL, NULL, N'09.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (143, NULL, NULL, NULL, N'10.45-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (144, NULL, NULL, NULL, N'10.30-12.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (145, NULL, NULL, NULL, N'10.00-12.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (146, NULL, NULL, NULL, N'09.00-12.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (147, NULL, NULL, NULL, N'09.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (148, NULL, NULL, NULL, N'10.00-12.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (149, NULL, NULL, NULL, N'09.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (150, NULL, NULL, NULL, N'10.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (151, NULL, NULL, NULL, N'10.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (152, NULL, NULL, NULL, N'10.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (153, NULL, NULL, NULL, N'09.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (154, NULL, NULL, NULL, N'03.45-09.09, 10.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (155, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (156, NULL, NULL, NULL, N'10.00-12.00, ', NULL, N'09.00-10.00, ')
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (157, NULL, NULL, NULL, NULL, N'09.00-11.00, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (158, NULL, NULL, NULL, N'09.00-11.30, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (159, NULL, NULL, NULL, NULL, N'08.00-10.00, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (160, NULL, NULL, NULL, NULL, N'08.00-10.00, 08.00-10.00, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (161, NULL, NULL, NULL, NULL, N'09.00-11.30, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (162, N'09.00-11.00, ', NULL, N'09.00-11.00, ', NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (163, NULL, NULL, N'09.30-12.00, ', NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (164, N'09.00-13.00, ', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (165, NULL, NULL, NULL, NULL, N'08.00-10.00, ', NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (166, NULL, NULL, N'09.30-12.00, ', NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (167, N'09.00-11.00, ', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (168, NULL, N'09.00-11.30, ', NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (169, NULL, N'09.00-11.30, ', NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (170, NULL, NULL, N'09.00-11.00, ', NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (171, NULL, N'09.00-11.00, ', NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (172, NULL, NULL, N'09.00-11.00, ', NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (173, NULL, NULL, NULL, N'09.00-11.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (174, NULL, NULL, NULL, N'09.00-10.00, ', NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (177, N'09.00-11.00, ', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (178, NULL, N'09.00-11.00, ', NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (179, NULL, N'09.00-11.30, ', NULL, NULL, NULL, NULL)
INSERT [dbo].[WeekDays] ([Code], [Sunday], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday]) VALUES (180, N'09.00-12.00, ', NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[WeekDays] OFF
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD FOREIGN KEY([DescriberUserCode])
REFERENCES [dbo].[Users] ([Code])
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD FOREIGN KEY([DescriptedUserCode])
REFERENCES [dbo].[Users] ([Code])
GO
ALTER TABLE [dbo].[ParkingSpot]  WITH CHECK ADD FOREIGN KEY([CityCode])
REFERENCES [dbo].[Cities] ([Code])
GO
ALTER TABLE [dbo].[ParkingSpot]  WITH CHECK ADD FOREIGN KEY([DaysSchedule])
REFERENCES [dbo].[WeekDays] ([Code])
GO
ALTER TABLE [dbo].[ParkingSpot]  WITH CHECK ADD FOREIGN KEY([UserCode])
REFERENCES [dbo].[Users] ([Code])
GO
ALTER TABLE [dbo].[ParkingSpotSearch]  WITH CHECK ADD FOREIGN KEY([CityCode])
REFERENCES [dbo].[Cities] ([Code])
GO
ALTER TABLE [dbo].[ParkingSpotSearch]  WITH CHECK ADD FOREIGN KEY([DaysSchedule])
REFERENCES [dbo].[WeekDays] ([Code])
GO
ALTER TABLE [dbo].[ParkingSpotSearch]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Code])
GO
ALTER TABLE [dbo].[SearchResults]  WITH CHECK ADD FOREIGN KEY([ResultPSCode])
REFERENCES [dbo].[ParkingSpot] ([Code])
GO
ALTER TABLE [dbo].[SearchResults]  WITH CHECK ADD FOREIGN KEY([SearchCode])
REFERENCES [dbo].[ParkingSpotSearch] ([Code])
GO
ALTER TABLE [dbo].[SearchResults]  WITH CHECK ADD FOREIGN KEY([Usercode])
REFERENCES [dbo].[Users] ([Code])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([PaymentDetails1])
REFERENCES [dbo].[PaymentDetails] ([Code])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([PaymentDetails2])
REFERENCES [dbo].[PaymentDetails] ([Code])
GO
USE [master]
GO
ALTER DATABASE [ParkMarkDB] SET  READ_WRITE 
GO
