--------------------------------updateSongData------------------------------------------------------------
CREATE PROCEDURE dbo.updateSongData( @albumId varchar(5) = null
									,@songId varchar(5) = null
									,@songName varchar(1024) = null
									,@length varchar(15) = null
									,@trackNumber varchar(15) = null
									,@genre varchar(100) = null
									)
AS

SET NOCOUNT ON
DECLARE @procName varchar(100), @lastError varchar(100)

SELECT @procName = OBJECT_NAME(@@PROCID)

iF( @albumId IS NULL OR LTRIM(RTRIM(@albumId)) LIKE '')
	BEGIN
		raiserror ('%s: Album Id is required for further processing', 16,-1, @procName)
		RETURN -1
	END

iF( @songId IS NULL OR LTRIM(RTRIM(@songId)) LIKE '')
	BEGIN
		raiserror ('%s: Song Id is required for further processing', 16,-1, @procName)
		RETURN -1
	END

UPDATE dbo.Song
SET title = ISNULL(@songName, title)
	,songLength = ISNULL(@length, songLength)
	,trackNumber = ISNULL(@trackNumber, trackNumber)
	,genre = ISNULL(@genre, genre)
	,updatedDate = GETDATE()
WHERE albumId = @albumId

SELECT @lastError = @@ERROR

IF(@lastError != 0)
	BEGIN
		raiserror ('%s: Error [%d] while updating song information for %s', 16,-1, @procName, @lastError, @songName)
		RETURN -1
	END

SET NOCOUNT OFF
----------------------------------insertSongData----------------------------------------------------------
CREATE PROCEDURE dbo.insertSongData(@albumId varchar(5) = null
									,@albumName varchar(100) = null
									,@songName varchar(200) = null
									,@length varchar(5) = null
									,@trackNumber varchar(10) = null
									,@genre varchar(25) = null
									--,@songId varchar(5) out
									)
AS
SET NOCOUNT ON
DECLARE @procName varchar(50), @lastError int, @songId int

SELECT @procName = OBJECT_NAME(@@PROCID)

iF( @albumId IS NULL OR LTRIM(RTRIM(@albumId)) LIKE '')
	BEGIN
		raiserror ('%s: Album Id is required for further processing', 16,-1, @procName)
		RETURN -1
	END
IF(@songName IS NULL OR LTRIM(RTRIM(@songName)) LIKE '')
	BEGIN
		raiserror ('%s: Song Name is required for further processing', 16,-1, @procName)
	END

	INSERT INTO dbo.Song(title, songLength, trackNumber, genre, albumId)
	VALUES
	(@songName, @length, @trackNumber, @genre, (SELECT Id FROM dbo.Album WHERE Id = @albumId))
	SELECT @lastError = @@ERROR, @songId = SCOPE_IDENTITY()
	IF(@lastError != 0)
		BEGIN
			raiserror ('%s: Error [%d] while SELECTing Album information for %s', 16,-1, @procName, @lastError, @songName)
			RETURN -1
		END
	SELECT @songId as SongId
SET NOCOUNT OFF
----------------------------getAlbumdata----------------------------------------------------------------
CREATE PROCEDURE getAlbumdata( @albumName VARCHAR(100))

AS

SET NOCOUNT ON

DECLARE @procName varchar(50), @lastError int
SELECT @procName = OBJECT_NAME(@@PROCID)

IF( @albumName IS NULL OR LTRIM(RTRIM(@albumName)) LIKE '')
	BEGIN
		raiserror ('%s: Album Name is required for further processing', 16,-1, @procName)
		RETURN -1
	END

	SET @albumName = '%' + @albumName + '%'

	SELECT a.Id AS albumId, a.Title AS albumName, ar.artistName, s.songId, s.title AS songName
			,s.songLength AS [length], s.trackNumber, s.genre
	FROM dbo.Song s
		INNER JOIN dbo.Album a ON a.Id = s.albumId
		INNER JOIN dbo.Artist ar ON ar.Id = a.artistId 
	WHERE a.Title LIKE @albumName
	SELECT @lastError = @@ERROR
	IF(@lastError != 0)
		BEGIN
			raiserror ('%s: Error [%d] while SELECTing Album information for %s', 16,-1, @procName, @lastError, @albumName)
			RETURN -1
		END
SET NOCOUNT OFF
----------------------------Create table----------------------------------------------------------------
USE [master]
GO

/****** Object:  Table [dbo].[Album]    Script Date: 12/17/2015 9:35:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Album](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[artistId] [int] NOT NULL,
	[Title] [varchar](1024) NULL,
	[createdDate] [datetime] NULL DEFAULT GETDATE(),
	[updatedDate] [datetime] NULL,
 CONSTRAINT [pk_albumId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Album]  WITH CHECK ADD  CONSTRAINT [fk_artistId] FOREIGN KEY([artistId])
REFERENCES [dbo].[Artist] ([Id])
GO

ALTER TABLE [dbo].[Album] CHECK CONSTRAINT [fk_artistId]
GO
----------------------------------------------------------------
USE [master]
GO

/****** Object:  Table [dbo].[Artist]    Script Date: 12/17/2015 9:37:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Artist](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[artistName] [varchar](200) NOT NULL,
	[createdDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
 CONSTRAINT [pk_artistId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Artist] ADD  DEFAULT (getdate()) FOR [createdDate]
GO
-------------------------------------------------------------------
USE [master]
GO

/****** Object:  Table [dbo].[Song]    Script Date: 12/17/2015 9:37:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Song](
	[songId] [int] IDENTITY(1,1) NOT NULL,
	[albumId] [int] NOT NULL,
	[title] [varchar](1024) NOT NULL,
	[songLength] [varchar](10) NOT NULL,
	[trackNumber] [varchar](15) NULL,
	[genre] [varchar](100) NULL,
	[createdDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
 CONSTRAINT [pk_songId] PRIMARY KEY CLUSTERED 
(
	[songId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Song] ADD  DEFAULT (getdate()) FOR [createdDate]
GO

ALTER TABLE [dbo].[Song]  WITH CHECK ADD  CONSTRAINT [fk_albumId] FOREIGN KEY([albumId])
REFERENCES [dbo].[Album] ([Id])
GO

ALTER TABLE [dbo].[Song] CHECK CONSTRAINT [fk_albumId]
GO

-----------------------iNSERT rECORD----------------------------------------------
insert into dbo.Song (title, songLength, trackNumber,genre, albumId) values
 ('Beautiful','4:04','1','Song' ,(select id from dbo.Album where title = 'smack that'))
insert into dbo.Song (title, songLength, trackNumber,genre, albumId) values
 ('You are Beautiful','4:04','1','Rap', (select id from dbo.Album where title = 'smack that'))
insert into dbo.Song (title, songLength, trackNumber,genre, albumId) values
 ('Turn Down For What','3.36','3','"Trance', (select id from dbo.Album where title = 'smack that'))
insert into dbo.Song (title, songLength, trackNumber,genre, albumId) values
 ('Animal','4.50','4','Trance', (select id from dbo.Album where title = 'smack that'))
insert into dbo.Song (title, songLength, trackNumber,genre, albumId) values
 ('Smack That','3.15','5','Rap', (select id from dbo.Album where title = 'smack that'))


      