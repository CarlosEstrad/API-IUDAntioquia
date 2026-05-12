/*
*******************************************************************************
PROYECTO: Music Playlist Management App
CANDIDATO: Carlos Estrada
FECHA: Mayo 2026
DESCRIPCIÓN: Script de creación de base de datos, tablas e índices.
*******************************************************************************
*/

-- 1. CREACIÓN DE LA BASE DE DATOS
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MusicPlaylistDB')
BEGIN
    CREATE DATABASE MusicPlaylistDB;
END
GO

USE MusicPlaylistDB;
GO

-- 2. ELIMINACIÓN DE TABLAS (En orden inverso por FKs para evitar errores de integridad)
IF OBJECT_ID('PlaylistSongs', 'U') IS NOT NULL DROP TABLE PlaylistSongs;
IF OBJECT_ID('Songs', 'U') IS NOT NULL DROP TABLE Songs;
IF OBJECT_ID('Playlists', 'U') IS NOT NULL DROP TABLE Playlists;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO

-- 3. CREACIÓN DE TABLAS

-- Entidad de Usuarios: Almacena credenciales y perfiles.
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Entidad de Playlists: Relación 1:N con Usuarios.
CREATE TABLE Playlists (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Playlists_Users FOREIGN KEY (UserId) 
        REFERENCES Users(Id) ON DELETE CASCADE
);

-- Entidad de Canciones: Catálogo maestro de música disponible.
CREATE TABLE Songs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(150) NOT NULL,
    Artist NVARCHAR(150) NOT NULL,
    Album NVARCHAR(150),
    Duration INT, -- Almacenado en segundos para facilitar cálculos
);

-- Tabla Intermedia: Relación M:N entre Playlists y Canciones.
CREATE TABLE PlaylistSongs (
    PlaylistId INT NOT NULL,
    SongId INT NOT NULL,
    AddedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (PlaylistId, SongId),
    CONSTRAINT FK_PS_Playlist FOREIGN KEY (PlaylistId) 
        REFERENCES Playlists(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PS_Song FOREIGN KEY (SongId) 
        REFERENCES Songs(Id) ON DELETE CASCADE
);
GO

-- 4. OPTIMIZACIÓN (Índices)

-- Mejora el rendimiento al filtrar playlists por usuario (escenario master-detail)
CREATE INDEX IX_Playlists_UserId ON Playlists(UserId);

-- Optimiza la búsqueda de canciones por título (búsqueda global)
CREATE INDEX IX_Songs_Title ON Songs(Title);

-- Índice en Email para optimizar el proceso de Login
CREATE INDEX IX_Users_Email ON Users(Email);
GO

-- 5. CARGA DE DATOS MAESTROS (SEEDING)
PRINT 'Insertando catálogo inicial de canciones...';

INSERT INTO Songs (Title, Artist, Album, Duration) 
VALUES  
('Blinding Lights', 'The Weeknd', 'After Hours', 200),
('Bohemian Rhapsody', 'Queen', 'A Night at the Opera', 354),
('Shape of You', 'Ed Sheeran', 'Divide', 233),
('Hotel California', 'Eagles', 'Hotel California', 391),
('Levitating', 'Dua Lipa', 'Future Nostalgia', 203),
('Save Your Tears', 'The Weeknd', 'After Hours', 215),
('Dynamite', 'BTS', 'Be', 199),
('STAY', 'The Kid LAROI & Justin Bieber', 'F*ck Love 3', 141),
('Dakiti', 'Bad Bunny & Jhay Cortez', 'El Último Tour Del Mundo', 205),
('Pepas', 'Farruko', 'La 167', 287);

PRINT 'Script ejecutado con éxito.';
GO