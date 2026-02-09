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
  ```

- Crea el procedimiento almacenado `sp_ListarTrabajadores`.
- Inserta 2 registros de prueba (Juan / María).

---

## 4. Configuración de la cadena de conexión

Archivo: `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR_SQL;Database=TrabajadoresPrueba;User Id=USUARIO;Password=CONTRASEÑA;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Reemplazar:

- `TU_SERVIDOR_SQL` por el nombre de tu servidor local (por ejemplo, `localhost` o `localhost\\SQLEXPRESS`).
- `USUARIO` y `CONTRASEÑA` por las credenciales que uses en SQL Server.

El contexto se configura en `Program.cs`:

```csharp
builder.Services.AddDbContext<TrabajadoresContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

## 5. Ejecución del proyecto

### Opción A: desde Visual Studio

1. Abrir la solución `Myper.Trabajadores.Web.sln`.
2. Establecer el proyecto **Myper.Trabajadores.Web** como proyecto de inicio.
3. Ejecutar con **F5**.
4. Ir a:

   - `https://localhost:<puerto>/Trabajadores`  
     (o usar el botón desde `Home/Index`).

### Opción B: desde línea de comandos

```bash
cd Myper.Trabajadores.Web/Myper.Trabajadores.Web
dotnet run
```

Ir en el navegador a `http://localhost:<puerto>/Trabajadores` (el puerto se muestra en consola).

---

## 6. Funcionalidades implementadas

### 6.1. Listado de trabajadores

Ruta: `GET /Trabajadores/Index`

- Obtiene los datos desde el procedimiento almacenado:

  ```csharp
  var trabajadores = await _context.Trabajadores
      .FromSqlRaw("EXEC sp_ListarTrabajadores")
      .ToListAsync();
  ```

- Muestra una tabla con:

  - Nombres, Apellidos
  - Tipo/Número de documento
  - Sexo
  - Fecha de nacimiento
  - Dirección
  - Foto (miniatura si existe)
  - Botones **Editar** y **Eliminar**

- Bonus implementado:
  - Filas azules (clase `table-primary`) si `Sexo == 'M'`.
  - Filas naranjas (clase `table-warning`) si `Sexo == 'F'`.

### 6.2. Filtro por sexo (Bonus)

En el listado se incluye un filtro por sexo:

- Combo `Todos / Masculino / Femenino`.
- Al seleccionar un valor, se envía `?sexo=M` o `?sexo=F` a la acción `Index`.
- El controlador filtra en memoria por `t.Sexo`.

### 6.3. Registro de nuevo trabajador

Ruta: `GET /Trabajadores/Create` y `POST /Trabajadores/Create`

- Formulario con validaciones por DataAnnotations.
- Campos requeridos: nombres, apellidos, tipo/numero documento, sexo, fecha de nacimiento.
- Campo opcional: foto (archivo) y dirección.
- La foto se guarda como archivo físico en `wwwroot/images` y se almacena la ruta en la BD.
- La propiedad `Activo` se inicializa en `true` y `FechaRegistro` con la fecha/hora actual.

### 6.4. Edición de trabajador

Ruta: `GET /Trabajadores/Edit/{id}` y `POST /Trabajadores/Edit/{id}`

- Carga los datos actuales del trabajador.
- Permite modificar todos los campos.
- Si se sube una nueva foto, se genera un nuevo archivo y se actualiza la ruta.

### 6.5. Eliminación (borrado lógico)

Ruta: `GET /Trabajadores/Delete/{id}` y `POST /Trabajadores/Delete/{id}`

- Muestra una vista de confirmación con el mensaje:

  > ¿Está seguro de eliminar el registro?

- No se borra el registro físicamente: se marca `Activo = 0`.
- El procedimiento `sp_ListarTrabajadores` solo muestra registros con `Activo = 1`.

---

## 7. QA – Pruebas funcionales

En la carpeta `QA` (o en un documento adjunto) se incluye una tabla con los casos de prueba:

- **Alta de trabajador** (datos válidos / faltantes / duplicado de documento).
- **Edición** (modificación de campos, cambio de foto).
- **Eliminación** (confirmación y verificación de que no aparece en el listado).
- **Filtro por sexo** (M/F/Todos).
- **Validación de campos obligatorios**.

Cada caso de prueba documenta:

- Input / pasos.
- Resultado esperado.
- Resultado obtenido.
- Evidencia (captura de pantalla).

---

## 8. Prototipo (UI)

Si se utilizó Figma / Adobe XD:

- Incluir aquí el enlace al prototipo:

  - `Figma: https://www.figma.com/file/...` (opcional)

Las pantallas diseñadas son:

- Listado de trabajadores.
- Registro de nuevo trabajador.
- Edición de trabajador.
- Confirmación de eliminación.

---

## 9. Video de explicación (Loom)

Se incluye un video breve (5–10 min) explicando:

- Prototipo de UI.
- Arquitectura del proyecto.
- Decisiones técnicas.
- Proceso de QA.
- Demostración del módulo en funcionamiento.

Enlace: `https://www.loom.com/share/........` (a completar).

---

## 10. Repositorio de GitHub

Repositorio público con el código fuente:

- `https://github.com/<tu-usuario>/Myper.Trabajadores.Web` (a completar).

Se han realizado commits claros y descriptivos siguiendo una secuencia lógica:

- `feat: crear estructura inicial de proyecto`
- `feat: agregar modelo Trabajador y DbContext`
- `feat: implementar CRUD de trabajadores`
- `feat: agregar filtro por sexo y colores en listado`
- `chore: actualizar README y script SQL`
