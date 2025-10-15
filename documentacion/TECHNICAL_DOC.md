# Arquitectura T�cnica del M�dulo AdventureWorks.Enterprise.Api

## Descripci�n General
AdventureWorks.Enterprise.Api es el backend principal del sistema de gesti�n AdventureWorks2014. Expone una API RESTful sobre .NET 8 para la administraci�n de recursos empresariales, ventas y producci�n, siguiendo buenas pr�cticas de arquitectura y seguridad.

## Estructura de Carpetas
- **Controllers/**: Controladores API para cada entidad principal (Employee, Product, SalesOrder, etc.).
- **DTOs/**: Data Transfer Objects para entrada/salida, desacoplando el modelo de datos de la API.
- **Entities/**: Modelos de datos que representan las tablas de la base de datos.
- **Data/**: AdventureWorksDbContext y configuraci�n de EF Core.
- **Scripts/**: Scripts SQL y procedimientos almacenados utilizados por la API.
- **Middleware/**: Componentes para seguridad y manejo de API Key.

## Principales Componentes
### 1. Controladores
Cada controlador implementa endpoints CRUD y consultas avanzadas. Ejemplo:
- **EmployeeController**: CRUD de empleados, consulta por departamento, integraci�n con SPs.
- **ProductController**: CRUD de productos, consulta por categor�a/subcategor�a.
- **SalesOrderController**: Gesti�n de �rdenes de venta, historial, top productos vendidos.

### 2. DTOs
Los DTOs definen los datos que se reciben y env�an en la API, evitando exponer directamente las entidades de base de datos. Se usan para validaci�n y mapeo seguro.

### 3. DbContext y EF Core
AdventureWorksDbContext gestiona la conexi�n a la base de datos SQL Server y el mapeo de entidades. Se configura en Program.cs para usar la cadena de conexi�n `DefaultConnection`.

### 4. Seguridad
- **API Key Middleware**: Todas las rutas requieren un encabezado `X-Api-Key` v�lido, configurado en appsettings.json.
- **Validaciones**: Los DTOs usan atributos como `[Required]`, `[StringLength]` para asegurar la integridad de los datos.

### 5. Integraci�n con SQL y SPs
Muchos endpoints ejecutan procedimientos almacenados (SPs) para l�gica compleja, como creaci�n de �rdenes, reportes avanzados y consultas agregadas. Los resultados se mapean a DTOs sin clave para facilitar el acceso.

### 6. Manejo de Errores
Todos los controladores implementan manejo de excepciones y devuelven respuestas estructuradas con informaci�n de error, mensaje y observaciones.

## Flujo de una Solicitud T�pica
1. El cliente realiza una solicitud HTTP a un endpoint (ej. POST /api/Employee/full).
2. El middleware valida la API Key.
3. El controlador recibe la solicitud, valida el DTO y ejecuta la l�gica (CRUD o SP).
4. El resultado se mapea a un DTO y se devuelve en una respuesta JSON estructurada.
5. Si ocurre un error, se devuelve un mensaje claro y el c�digo HTTP adecuado.

## Pruebas y Calidad
El proyecto incluye pruebas unitarias en AdventureWorks.Enterprise.Api.Tests para validar casos de �xito y fallo en los controladores principales.

## Extensibilidad
La arquitectura modular permite agregar nuevos controladores, entidades y SPs siguiendo el mismo patr�n. El uso de DTOs y middleware facilita la integraci�n y el mantenimiento.

---
**�ltima actualizaci�n:** Octubre 2025
