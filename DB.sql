USE [master]
GO
/****** Object:  Database [ParkMarkDB]    Script Date: 15/07/2021 20:01:32 ******/
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
/****** Object:  Table [dbo].[Cities]    Script Date: 15/07/2021 20:01:32 ******/
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
/****** Object:  Table [dbo].[Feedbacks]    Script Date: 15/07/2021 20:01:33 ******/
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
/****** Object:  Table [dbo].[ParkingSpot]    Script Date: 15/07/2021 20:01:33 ******/
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
/****** Object:  Table [dbo].[ParkingSpotSearch]    Script Date: 15/07/2021 20:01:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParkingSpotSearch](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[MyLocationAddress] [varchar](120) NULL,
	[Place_id] [varchar](100) NULL,
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
/****** Object:  Table [dbo].[PaymentDetails]    Script Date: 15/07/2021 20:01:33 ******/
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
/****** Object:  Table [dbo].[SearchResults]    Script Date: 15/07/2021 20:01:33 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 15/07/2021 20:01:33 ******/
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
/****** Object:  Table [dbo].[WeekDays]    Script Date: 15/07/2021 20:01:33 ******/
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
