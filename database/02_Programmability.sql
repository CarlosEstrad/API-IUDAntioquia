/*
*******************************************************************************
PROYECTO: Music Playlist Management App
CANDIDATO: Carlos Estrada
FECHA: Mayo 2026
DESCRIPCIÓN: Capa de lógica de base de datos (Stored Procedures y Views).
*******************************************************************************
*/

USE MusicPlaylistDB;
GO

-- =============================================================================
-- 1. VISTAS (CAPA DE CONSULTA)
-- =============================================================================

-- Vista para catálogo de canciones con duración formateada (MM:SS)
IF OBJECT_ID('vw_GetAllSongs', 'V') IS NOT NULL DROP VIEW vw_GetAllSongs;
GO
CREATE VIEW vw_GetAllSongs AS
SELECT 
    Id, 
    Title, 
    Artist, 
    Album, 
    CAST(Duration / 60 AS VARCHAR(5)) + ':' + 
    RIGHT('0' + CAST(Duration % 60 AS VARCHAR(2)), 2) AS DurationFormat,
    Duration AS DurationSeconds
FROM Songs;
GO

-- Vista detallada: Master-Detail de Playlists con sus canciones
IF OBJECT_ID('vw_GetPlaylistsSongs', 'V') IS NOT NULL DROP VIEW vw_GetPlaylistsSongs;
GO
CREATE VIEW vw_GetPlaylistsSongs AS
SELECT 
    P.Id AS PlaylistId, 
    P.UserId, 
    P.Name AS PlaylistName, 
    P.Description,
    P.CreatedAt,
    U.Username,
    S.Id AS SongId,
    S.Album,
    S.Artist,
    S.Title,
    CAST(S.Duration / 60 AS VARCHAR(5)) + ':' + 
    RIGHT('0' + CAST(S.Duration % 60 AS VARCHAR(2)), 2) AS DurationFormat
FROM Playlists P
INNER JOIN Users U ON U.Id = P.UserId
LEFT JOIN PlaylistSongs PS ON PS.PlaylistId = P.Id
LEFT JOIN Songs S ON S.Id = PS.SongId;
GO

-- =============================================================================
-- 2. PROCEDIMIENTOS ALMACENADOS (CAPA DE PERSISTENCIA)
-- =============================================================================

-- GESTIÓN DE USUARIOS ---------------------------------------------------------

CREATE OR ALTER PROCEDURE sp_CreateUser
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Users (Username, Email, PasswordHash)
        VALUES (@Username, @Email, @PasswordHash);
        SELECT SCOPE_IDENTITY() AS NewId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- GESTIÓN DE PLAYLISTS --------------------------------------------------------

CREATE OR ALTER PROCEDURE sp_CreatePlaylist
    @UserId INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Playlists (UserId, Name, Description, CreatedAt)
    VALUES (@UserId, @Name, @Description, GETDATE());
    SELECT SCOPE_IDENTITY() AS NewId;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdatePlaylist
    @Id INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Playlists 
    SET Name = @Name, Description = @Description
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_DeletePlaylist
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    -- El borrado de PlaylistSongs se maneja por el ON DELETE CASCADE del FK
    DELETE FROM Playlists WHERE Id = @Id;
END;
GO

-- GESTIÓN DE CANCIONES Y ASOCIACIONES -----------------------------------------

CREATE OR ALTER PROCEDURE sp_AddSongToPlaylist
    @PlaylistId INT,
    @SongId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM PlaylistSongs WHERE PlaylistId = @PlaylistId AND SongId = @SongId)
    BEGIN
        INSERT INTO PlaylistSongs (PlaylistId, SongId)
        VALUES (@PlaylistId, @SongId);
    END
END;
GO

CREATE OR ALTER PROCEDURE sp_RemoveSongFromPlaylist
    @PlaylistId INT,
    @SongId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PlaylistSongs 
    WHERE PlaylistId = @PlaylistId AND SongId = @SongId;
END;
GO

CREATE OR ALTER PROCEDURE sp_CreateSong
    @Title NVARCHAR(150),
    @Artist NVARCHAR(150),
    @Album NVARCHAR(150),
    @Duration INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Songs (Title, Artist, Album, Duration)
    VALUES (@Title, @Artist, @Album, @Duration);
    SELECT SCOPE_IDENTITY() AS NewId;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateSong
    @Id INT,
    @Title NVARCHAR(150),
    @Artist NVARCHAR(150),
    @Album NVARCHAR(150),
    @Duration INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Songs 
    SET Title = @Title, Artist = @Artist, Album = @Album, Duration = @Duration
    WHERE Id = @Id;
END;
GO