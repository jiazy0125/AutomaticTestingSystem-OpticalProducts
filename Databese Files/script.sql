USE [master]
GO
/****** Object:  Database [NJSFDB]    Script Date: 2020/5/15 10:03:05 ******/
CREATE DATABASE [NJSFDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NJSFDB', FILENAME = N'E:\ZhanyongJia\MSSQLSRV\MSSQL15.MSSQLSERVER\MSSQL\DATA\NJSFDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NJSFDB_log', FILENAME = N'E:\ZhanyongJia\MSSQLSRV\MSSQL15.MSSQLSERVER\MSSQL\DATA\NJSFDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NJSFDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NJSFDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NJSFDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NJSFDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NJSFDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NJSFDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NJSFDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [NJSFDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NJSFDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NJSFDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NJSFDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NJSFDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NJSFDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NJSFDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NJSFDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NJSFDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NJSFDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NJSFDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NJSFDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NJSFDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NJSFDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NJSFDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NJSFDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NJSFDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NJSFDB] SET RECOVERY FULL 
GO
ALTER DATABASE [NJSFDB] SET  MULTI_USER 
GO
ALTER DATABASE [NJSFDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NJSFDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NJSFDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NJSFDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NJSFDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'NJSFDB', N'ON'
GO
ALTER DATABASE [NJSFDB] SET QUERY_STORE = OFF
GO
USE [NJSFDB]
GO
/****** Object:  Table [dbo].[PartNumberConfig]    Script Date: 2020/5/15 10:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartNumberConfig](
	[GUID] [varchar](50) NOT NULL,
	[PartNumber] [varchar](20) NOT NULL,
	[ProductType] [varchar](50) NULL,
	[ProcessGuid] [varchar](50) NULL,
	[Description] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessInfo]    Script Date: 2020/5/15 10:03:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessInfo](
	[GUID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](100) NULL,
	[Creator] [varchar](20) NULL,
	[CreateTime] [varchar](50) NULL,
	[ProductGuid] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductModel]    Script Date: 2020/5/15 10:03:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductModel](
	[GUID] [varchar](50) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Description] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubItemInfo]    Script Date: 2020/5/15 10:03:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubItemInfo](
	[GUID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](100) NULL,
	[Sequence] [int] NULL,
	[Parent] [varchar](50) NULL,
	[Independent] [bit] NULL,
	[AsVariable] [bit] NULL,
	[LowerOperator] [int] NULL,
	[LowerLimit] [varchar](50) NULL,
	[UpperOperator] [int] NULL,
	[UpperLimit] [varchar](50) NULL,
	[SelectedProcedure] [varchar](50) NULL,
	[OptionSelected] [int] NULL,
	[OptionParameters] [varchar](500) NULL,
	[DelayMS] [int] NULL,
	[VariantName] [varchar](50) NULL,
	[NeedBeSaved] [bit] NULL,
	[SourceData] [varchar](50) NULL,
	[CustomDefine] [bit] NULL,
	[CustomData] [varchar](200) NULL,
 CONSTRAINT [PK_TestItemInfo] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TablesDefine]    Script Date: 2020/5/15 10:03:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TablesDefine](
	[GUID] [varchar](50) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[TableIndex] [varchar](10) NULL,
	[Description] [varchar](200) NULL,
	[StartAddress] [varchar](10) NULL,
	[Parent] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopItemInfo]    Script Date: 2020/5/15 10:03:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopItemInfo](
	[GUID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](100) NULL,
	[Independent] [bit] NULL,
	[Sequence] [int] NULL,
	[Parent] [varchar](50) NULL,
 CONSTRAINT [PK_TestGroupInfo] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 2020/5/15 10:03:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[GUID] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Authority] [int] NOT NULL,
	[Remark] [varchar](500) NULL
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GUID参数唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'GUID'
GO
USE [master]
GO
ALTER DATABASE [NJSFDB] SET  READ_WRITE 
GO
