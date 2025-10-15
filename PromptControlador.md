# Instrucciones para generación automática de controladores

El objetivo es poder gestionar las entidades de la base de datos AdventureWorks2014. Te estaré 
compartiendo diferentes contextos que corresponden al esquema que tienen 
las tablas en la base de datos en los scripts T-SQL.

**IMPORTANTE:**  
-- PEGA AQUÍ EL SCRIPT COMPLETO DE LA TABLA (por ejemplo, el CREATE TABLE ...) --

Por favor, antes de generar código, si tienes dudas pregunta o confirma que entendiste las instrucciones.

Utilizando el esquema de [HumanResources].[Employee] realiza lo siguiente: 

1. Utiliza el valor de la entidad para nombrar el controlador y las clases relacionadas.
2. Trabaja en el proyecto AdventureWorksAPI.Enterprise.Api
3. Utiliza Contributing.md para los estándares de codificación.
4. Modifica el archivo `appsettings.json`. Si no existe una cadena de conexión llamada `DefaultConnection` créala apuntando a la base de datos [AdventureWorks2014] en mi servidor local SQL Express .\SQLEXPRESS utilizando seguridad integrada.  
   Ejemplo:  
   `"Server=.\\SQLEXPRESS;Database=AdventureWorks2014;Integrated Security=True;TrustServerCertificate=True"`
5. Si no existe la clase `AdventureWorksDbContext.cs` créala y que herede de `DbContext`. Incluye un constructor que acepte `DbContextOptions` y un `DbSet` para la entidad actual.
6. Configura la inyección de dependencias en `Program.cs`. Registra el `AdventureWorksDbContext` para que use SQL Server con la cadena de conexión `DefaultConnection`.
7. Crea un controlador. El nombre del controlador debe ser el nombre de la entidad seguido de la palabra `Controller`. El controlador utilizará `AdventureWorksDbContext`.
   - El controlador debe implementar operaciones CRUD (crear, leer, actualizar, eliminar) para la entidad correspondiente.
   - Usa DTOs para entrada/salida y no expongas directamente las entidades del modelo.
   - Aplica validaciones con atributos como `[Required]`, `[StringLength]`, etc.
   - Maneja errores y devuelve respuestas HTTP adecuadas.
8. Implementa seguridad con API Key: 
   - En `appsettings.json`, agrega una sección `ApiSetting` con una propiedad `ApiKey` y un valor secreto que tu generes aleatoriamente.
   - Crea un middleware llamado `ApiKeyMiddleware.cs` que lea la clave del encabezado `X-Api-Key` de la solicitud y la compare con el valor configurado en `appsettings.json`, si no coincide debe devolver un error 401.
   - Registra el middleware en `Program.cs` para que sea utilizado en cada API. 
9. Si tienes dudas, pídelas antes de generar el código. Confirma que has entendido las instrucciones antes de continuar.

---

> Cuando quieras generar otro controlador, solo agrega el script de la entidad que utilizarás.

## ¿Cómo utilizar este archivo?

1. Copia el contenido de este archivo (incluyendo las instrucciones y el bloque de entidad) y pégalo en Copilot.
2. Cambia el nombre y las propiedades de la entidad por la que quieras generar.
3. La IA leerá el prompt, entenderá el contexto y generará todo el código necesario para la entidad que le indiques (controlador, configuración, middleware, etc.).
4. Si quieres generar otro controlador, simplemente cambia la sección de la entidad y repite el proceso.

---