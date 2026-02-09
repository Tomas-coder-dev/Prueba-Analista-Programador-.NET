# Módulo de Mantenimiento de Trabajadores – Prueba Técnica MYPER

Este proyecto implementa el módulo de **mantenimiento de trabajadores** solicitado en la prueba técnica para el puesto de **Analista Programador .NET** en **MYPER Software**.

La solución está desarrollada con:

- **.NET 8** (ASP.NET Core MVC)
- **Entity Framework Core** + **SQL Server**
- **Bootstrap 5** (estilos por defecto del template MVC)
- Procedimientos almacenados en SQL Server

---

## 1. Arquitectura general

El proyecto es una aplicación **ASP.NET Core MVC** tradicional:

- **Capa Web**
  - `Controllers/TrabajadoresController.cs`  
    CRUD de trabajadores + filtrado por sexo.
  - `Views/Trabajadores/`  
    Vistas Razor para:
    - `Index` (listado + filtro + acciones)
    - `Create`
    - `Edit`
    - `Delete`
  - `Views/Home/`  
    Página inicial con un botón que redirige a Trabajadores.

- **Capa de Datos**
  - `Data/TrabajadoresContext.cs`  
    DbContext de EF Core configurado para la tabla `Trabajadores`.
  - `Models/Trabajador.cs`  
    Entidad que representa la tabla `Trabajadores`, con anotaciones de validación.

- **Base de datos**
  - BD: `TrabajadoresPrueba`
  - Tabla: `Trabajadores`
  - Procedimiento almacenado: `sp_ListarTrabajadores`
  - Script completo incluido en `script_trabajadores.sql`.

---

## 2. Requisitos previos

- **.NET SDK 8.x**
- **SQL Server** (Developer, Express o similar)
- Herramienta de administración SQL (SQL Server Management Studio, Azure Data Studio, etc.)

---

## 3. Preparación de la base de datos

1. Abrir SQL Server Management Studio.
2. Ejecutar el script `script_trabajadores.sql` (adjunto en la carpeta raíz del proyecto).

El script:

- Crea la base de datos `TrabajadoresPrueba`.
- Crea la tabla `Trabajadores` con los campos:

  ```text
  Id, Nombres, Apellidos, TipoDocumento, NumeroDocumento,
  Sexo, FechaNacimiento, Foto, Direccion, FechaRegistro, Activo
