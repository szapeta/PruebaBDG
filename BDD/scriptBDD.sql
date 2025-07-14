IF DB_ID('pruebap') IS NULL
BEGIN
    CREATE DATABASE pruebap;
END
GO

USE pruebap;
GO

--tabla puesto
CREATE TABLE puesto (
    idpuesto INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(100) NOT NULL
);
GO

--tabla empleado
CREATE TABLE empleado (
    idempleado INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(100) NOT NULL,
    idpuesto INT NULL FOREIGN KEY REFERENCES puesto(idpuesto),
    idjefe INT NULL FOREIGN KEY REFERENCES empleado(idempleado),
    fecha_registro DATETIME NOT NULL
);
GO

-- sp puesto

CREATE PROCEDURE sp_puesto_insertar
    @nombre VARCHAR(100)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM puesto WHERE nombre = @nombre)
    BEGIN
        RAISERROR('El nombre del puesto ya existe.', 16, 1);
        RETURN;
    END

    INSERT INTO puesto (nombre) VALUES (@nombre);
END;
GO



CREATE PROCEDURE sp_puesto_getall
AS
BEGIN
    SELECT idpuesto, nombre FROM puesto ORDER BY nombre;
END;
GO


CREATE PROCEDURE sp_puestoxid
    @idpuesto INT
AS
BEGIN
    SELECT idpuesto, nombre FROM puesto WHERE idpuesto = @idpuesto;
END;
GO


-- sp empleado

CREATE PROCEDURE sp_Empleado_Insertar
    @nombre VARCHAR(100),
    @idpuesto INT = NULL,
    @idjefe INT = NULL
AS
BEGIN
    INSERT INTO empleado (nombre, idpuesto, idjefe, fecha_registro)
    VALUES (@nombre, @idpuesto, @idjefe, GETDATE());
END;
GO


CREATE PROCEDURE sp_Empleado_ObtenerTodos
AS
BEGIN
    SELECT * FROM empleado;
END;
GO


CREATE PROCEDURE sp_Empleado_ObtenerPorId
    @idempleado INT
AS
BEGIN
    SELECT * FROM empleado WHERE idempleado = @idempleado;
END;
GO


CREATE PROCEDURE sp_Empleado_Actualizar
    @idempleado INT,
    @nombre VARCHAR(100),
    @idpuesto INT = NULL,
    @idjefe INT = NULL
AS
BEGIN
    UPDATE empleado
    SET nombre = @nombre,
        idpuesto = @idpuesto,
        idjefe = @idjefe
    WHERE idempleado = @idempleado;
END;
GO
