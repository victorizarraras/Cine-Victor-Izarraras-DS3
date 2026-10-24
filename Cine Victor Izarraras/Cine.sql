IF DB_ID('Cine') IS NULL
    CREATE DATABASE Cine;
GO

USE Cine;
GO

IF OBJECT_ID('dbo.Clasificacion', 'U') IS NULL
CREATE TABLE Clasificacion (
    IdClasificacion INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(10) NOT NULL,
    Descripcion NVARCHAR(100) NOT NULL
);

IF OBJECT_ID('dbo.Genero', 'U') IS NULL
CREATE TABLE Genero (
    IdGenero INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);

IF OBJECT_ID('dbo.Pelicula', 'U') IS NULL
CREATE TABLE Pelicula (
    IdPelicula INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(100) NOT NULL,
    Duracion INT NOT NULL,
    FechaEstreno DATE NOT NULL,
    IdClasificacion INT NOT NULL REFERENCES Clasificacion(IdClasificacion),
    IdGenero INT NOT NULL REFERENCES Genero(IdGenero)
);

IF OBJECT_ID('dbo.Estado', 'U') IS NULL
CREATE TABLE Estado (
    IdEstado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);

IF OBJECT_ID('dbo.Ciudad', 'U') IS NULL
CREATE TABLE Ciudad (
    IdCiudad INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    IdEstado INT NOT NULL REFERENCES Estado(IdEstado)
);

IF OBJECT_ID('dbo.Sucursal', 'U') IS NULL
CREATE TABLE Sucursal (
    IdSucursal INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Direccion NVARCHAR(100) NOT NULL,
    IdCiudad INT NOT NULL REFERENCES Ciudad(IdCiudad)
);

IF OBJECT_ID('dbo.Sala', 'U') IS NULL
CREATE TABLE Sala (
    IdSala INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(30) NOT NULL,
    Capacidad INT NOT NULL,
    IdSucursal INT NOT NULL REFERENCES Sucursal(IdSucursal)
);

IF OBJECT_ID('dbo.TipoBoleto', 'U') IS NULL
CREATE TABLE TipoBoleto (
    IdTipoBoleto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(30) NOT NULL,
    Precio DECIMAL(8,2) NOT NULL
);

IF OBJECT_ID('dbo.Funcion', 'U') IS NULL
CREATE TABLE Funcion (
    IdFuncion INT IDENTITY(1,1) PRIMARY KEY,
    IdPelicula INT NOT NULL REFERENCES Pelicula(IdPelicula),
    IdSala INT NOT NULL REFERENCES Sala(IdSala),
    FechaHora DATETIME NOT NULL
);

IF OBJECT_ID('dbo.Cliente', 'U') IS NULL
CREATE TABLE Cliente (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL
);

IF OBJECT_ID('dbo.Venta', 'U') IS NULL
CREATE TABLE Venta (
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente INT NOT NULL REFERENCES Cliente(IdCliente),
    IdFuncion INT NOT NULL REFERENCES Funcion(IdFuncion),
    IdTipoBoleto INT NOT NULL REFERENCES TipoBoleto(IdTipoBoleto),
    Cantidad INT NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    FechaVenta DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- datos de ejemplo para los catálogos (solo si las tablas están vacías)
IF NOT EXISTS (SELECT 1 FROM Clasificacion)
INSERT INTO Clasificacion (Nombre, Descripcion) VALUES
    (N'AA', N'Para todo público'),
    (N'A', N'Para todo público'),
    (N'B', N'Mayores de 12 años'),
    (N'B15', N'Mayores de 15 años'),
    (N'C', N'Mayores de 18 años'),
    (N'D', N'Solo adultos');

IF NOT EXISTS (SELECT 1 FROM Genero)
INSERT INTO Genero (Nombre) VALUES
    (N'Acción'), (N'Aventura'), (N'Ciencia Ficción'), (N'Comedia'),
    (N'Drama'), (N'Infantil'), (N'Romántica'), (N'Terror');

IF NOT EXISTS (SELECT 1 FROM TipoBoleto)
INSERT INTO TipoBoleto (Nombre, Precio) VALUES
    (N'Adulto', 80.00), (N'Niño', 60.00), (N'Adulto mayor', 55.00);
GO
