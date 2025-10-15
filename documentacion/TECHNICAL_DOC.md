# Arquitectura Técnica del Módulo AdventureWorks.Enterprise.Api

## Descripción General
AdventureWorks.Enterprise.Api es el backend principal del sistema de gestión AdventureWorks2014. Expone una API RESTful sobre .NET 8 para la administración de recursos empresariales, ventas y producción, siguiendo buenas prácticas de arquitectura y seguridad.

## Estructura de Carpetas
- **Controllers/**: Controladores API para cada entidad principal (Employee, Product, SalesOrder, etc.).
- **DTOs/**: Data Transfer Objects para entrada/salida, desacoplando el modelo de datos de la API.
- **Entities/**: Modelos de datos que representan las tablas de la base de datos.
- **Data/**: AdventureWorksDbContext y configuración de EF Core.
- **Scripts/**: Scripts SQL y procedimientos almacenados utilizados por la API.
- **Middleware/**: Componentes para seguridad y manejo de API Key.

## Principales Componentes
### 1. Controladores
Cada controlador implementa endpoints CRUD y consultas avanzadas. Ejemplo:
- **EmployeeController**: CRUD de empleados, consulta por departamento, integración con SPs.
- **ProductController**: CRUD de productos, consulta por categoría/subcategoría.
- **SalesOrderController**: Gestión de órdenes de venta, historial, top productos vendidos.

### 2. DTOs
Los DTOs definen los datos que se reciben y envían en la API, evitando exponer directamente las entidades de base de datos. Se usan para validación y mapeo seguro.

### 3. DbContext y EF Core
AdventureWorksDbContext gestiona la conexión a la base de datos SQL Server y el mapeo de entidades. Se configura en Program.cs para usar la cadena de conexión `DefaultConnection`.

### 4. Seguridad
- **API Key Middleware**: Todas las rutas requieren un encabezado `X-Api-Key` válido, configurado en appsettings.json.
- **Validaciones**: Los DTOs usan atributos como `[Required]`, `[StringLength]` para asegurar la integridad de los datos.

### 5. Integración con SQL y SPs
Muchos endpoints ejecutan procedimientos almacenados (SPs) para lógica compleja, como creación de órdenes, reportes avanzados y consultas agregadas. Los resultados se mapean a DTOs sin clave para facilitar el acceso.

### 6. Manejo de Errores
Todos los controladores implementan manejo de excepciones y devuelven respuestas estructuradas con información de error, mensaje y observaciones.

## Flujo de una Solicitud Típica
1. El cliente realiza una solicitud HTTP a un endpoint (ej. POST /api/Employee/full).
2. El middleware valida la API Key.
3. El controlador recibe la solicitud, valida el DTO y ejecuta la lógica (CRUD o SP).
4. El resultado se mapea a un DTO y se devuelve en una respuesta JSON estructurada.
5. Si ocurre un error, se devuelve un mensaje claro y el código HTTP adecuado.

## Pruebas y Calidad
El proyecto incluye pruebas unitarias en AdventureWorks.Enterprise.Api.Tests para validar casos de éxito y fallo en los controladores principales.

## Extensibilidad
La arquitectura modular permite agregar nuevos controladores, entidades y SPs siguiendo el mismo patrón. El uso de DTOs y middleware facilita la integración y el mantenimiento.

---
**Última actualización:** Octubre 2025
