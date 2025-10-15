# FASE 2 BACKEND

## RECURSOS HUMANOS (Human Resources)

El objetivo es poder gestionar las entidades de la base de datos AdventureWorks2014. Te estaré 
compartiendo diferentes contextos que corresponden al esquema que tienen 
las tablas en la base de datos en los scripts T-SQL.
 
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
   - En `appsettings.json`, si no existe en `ApiSetting` una propiedad llamada `ApiKey` agregala y dale un valor secreto que tu generes aleatoriamente.
   - Si no existe un middleware llamado `ApiKeyMiddleware.cs` agregalo y que lea la clave del encabezado `X-Api-Key` de la solicitud y la compare con el valor configurado en `appsettings.json`, si no coincide debe devolver un error 401.
   - Registra el middleware en `Program.cs` para que sea utilizado en cada API. 
9. No inicies hasta que me indiques que entendiste y no tengas dudas.

## Ventas (Sales)
	Trabaja en el proyecto AdventureWorks.Enterprise.Api,
	utilizando la base de datos AdventureWorks2014 trabaja basandote
	en el siguiente esquema
		- Ventas (Sales): Lógica transaccional y relaciones maestro-detalle.
	Trabajaras con diferentes entidades de la base de datos, el contexto de cada entidade
	está en Sales.md. Utiliza las siguientes entidades:
	- Sales.SalesOrderHeader
	- Sales.SalesOrderDetail
	- Sales.Customer
	- Sales.SalesPerson
	- Production.Product
	
	Basandote en esas entidades vas a crear los controladores para su mantenimiento.
 
	1. Utiliza Contributing.md para los estándares de la codificación.
	2. Implementa el ApiKey para la seguridad de los métodos de los controladores.
	3. SalesOrderController: Gestión de órdenes de venta
	   utilizando Sales.SalesOrderHeader y Sales.SalesOrderDetail
		3.1 Crea un Stored Procedure para la creación de ordenes:
			- El sp debe pedir como parametros los datos para el encabezado y el detalle
			  de la orden.
			- Agrega transacciones. 
			- Utiliza try/catch para el manejo de errores.
			- Guarda el Sp con el nombre sp_CreacionOrdenes 
			- Graba el script del sp en la carpeta scripts.
		3.2 Crea DTOs para los datos de entrada y salida.
		3.3 El controlador debe de tener los siguientes métodos: 
			- Crear una nueva orden (utiliza el sp sp_CreacionOrdenes)
			- Anular una orden.
			- Consultar una orden.
			- Método que obtenga el historial de ordenes por rango de fecha.
	4. CustomerController: Manejo de clientes utilizando Sales.Customer
		4.1 Crea DTOs para los datos de entrada y salida.
		4.2 El controlador debe de tener los siguientes métodos:
			- Registrar nuevo cliente.
			- Consultar cliente.
			- Actualizar cliente.
			- Listar clientes.
			- Listar ordenes por cliente. 
	5. SalesPersonController: Manejo de vendedores utilizando Sales.SalesPerson
		5.1 Crea DTOs para los datos de entrada y salida.
		5.2 El controlador debe de tener los siguientes métodos:
			- Consultar vendedor.
			- Listar ordenes por vendedor. 
	6. ProductController: consulta de productos.
		6.1 Crea DTOs para los datos de entrada y salida.
		6.2 El controlador debe tener los siguientes métodos:
			- Consulta de producto.
			- Listar productos. 
	7. No inicies hasta que me indiques que entendiste y no tengas dudas.
## Producción (Production)
	Trabaja en el proyecto AdventureWorks.Enterprise.Api,
	utilizando la base de datos AdventureWorks2014 trabaja basandote
	en el siguiente esquema
		- Producción (Production): Gestión de productos y manufactura.
	Trabajaras con diferentes entidades de la base de datos, el contexto de cada entidade
	está en Production.md. Utiliza las siguientes entidades:
	- Production.Product
	- Production.ProductCategory
	- Production.ProductSubcategory
	
	Basandote en esas entidades vas a crear los controladores para su mantenimiento.

	1. Utiliza Contributing.md para los estándares de la codificación.
	2. Implementa el ApiKey para la seguridad de los métodos de los controladores.
	3. ProductController: Gestión de productos utilizando Production.Product
		3.1 Crea DTOs para los datos de entrada y salida.
		3.2 El controlador debe de tener los siguientes métodos: 
			- Crear un nuevo producto.
			- Actualizar un producto.
			- Consultar un producto.
			- Listar productos.
			- Listar productos por categoría y subcategoría.
	4. ProductCategoryController: Manejo de categorías utilizando Production.ProductCategory
		4.1 Crea DTOs para los datos de entrada y salida.
		4.2 El controlador debe de tener los siguientes métodos:
			- Crear una nueva categoría.
			- Actualizar una categoría.
			- Consultar una categoría.
			- Listar categorías.
			- Listar subcategorías por categoría.
	5. ProductSubcategoryController: Manejo de subcategorías utilizando Production.ProductSubcategory
		5.1 Crea DTOs para los datos de entrada y salida.
		5.2 El controlador debe de tener los siguientes métodos:
			- Crear una nueva subcategoría.
			- Actualizar una subcategoría.
			- Consultar una subcategoría.
			- Listar subcategorías.
			- Listar productos por subcategoría.
	6. No inicies hasta que me indiques que entendiste y no tengas dudas.)
## Consultas SQL Avanzadas
### RRHH - Empleados con más tiempo en su departamento actual. 
	1.	Crea un stored procedure en la base de datos AdventureWorks2014. 
		El objetivo del sp es devolver el listado de empleados que tengan más tiempo en su departamento actual, utiliza
		las siguientes entidades:
			- HumanResources.Employee: contiene la información de los empleados
			- HumanResources.EmployeeDepartmentHistory: historial de empleados en su departamento
			- HumanResources.Department: información de los departamentos
			- Person.Person: nombre del empleado	
	2. Para las relaciones entre entidades utiliza:
		- HumanResources.EmployeeDepartmentHistory con HumanResources.Employee usando BusinessEntityID
		- HumanResources.EmployeeDepartmentHistory con HumanResources.Department usando DepartmentID
		- HumanResources.Employee con HumanResources.Person usando BusinessEntityID
	3. Que el stored procedure pida como parámetro la cantidad de personas que el usuario desea visualizar
	4. Agrega manejo de errores dentro del sp.   
	5. Guarda el stored procedure con el nombre sp_DepartamentoEmpleado
	6. Crea un archivo Dto con los campos que devuelve el sp. 
	7. Crea un endpoint dentro de EmployeeController que devuelva los datos del stored procedure, utiliza Contributing.md para
	   los estándares que tendrá la función. 
	8. La url para consumir el servicio será: /api/EmployeeDepartment/{parameter}
	9. Antes de crear el sp indicame si tienes una duda y si comprendiste lo que te estoy solicitando.  
### Ventas - Top 10 de productos más vendidos.
1. Crea un stored procedure con el nombre sp_TopProductosVentas,el objetivo del sp es obtener el top 10 de productos más vendidos. 
2. Para crear el sp utiliza las entidades que están en el archivo sales.md
3. La consulta debe devolver los siguientes campos:
	- ProductID (Código)
	- ProductName (Descripción)
	- UnitPrice (Precio unitario)
	- TotalQuantitySold (Unidades Vendidas)
	- TotalSalesAmount (Total Vendido)
	- CategoryName (Categoría)
	- SubCategoryName (Subcategoría)
4. Crea un endpoint dentro de SalesOrderController que devuelva los datos del stored procedure, utiliza Contributing.md para
los estándares que tendrá la función. 
5. La url para consumir el servicio será: /api/SalerOrderController/TopProducts
6. Antes de crear el sp indicame lo que vas a realizar y si tienes alguna duda.
### Producción - Productos con bajo inventario
Crea un stored procedure en la base de datos AdventureWorks2014. 
		El objetivo del sp es devolver un listado de productos con bajo inventario, utiliza
		las siguientes entidades:
			- Production.Product
			- Production.ProductInventory
			- Production.ProductSubcategory
			- Production.ProductCategory
			- Production.Location
	2. Para las relaciones entre entidades utiliza:
		- Production.ProductInventory con Production.Product usando ProductID
		- Production.Product con Production.ProductSubcategory usando ProductSubcategoryID
		- Production.ProductSubcategory  con Production.ProductCategory  usando ProductCategoryID
		- Production.ProductInventory  con Production.Location  usando LocationID
	3. Que el stored procedure pida como parámetro la cantidad de unidades
	4. Agrega manejo de errores dentro del sp.   
	5. Guarda el stored procedure con el nombre sp_BajoInventario
	6. Crea un archivo Dto con los campos que devuelve el sp. 
	7. Crea un endpoint dentro de ProductoInventoryController que devuelva los datos del stored procedure, utiliza Contributing.md para
	   los estándares que tendrá la función. 
	8. Antes de crear el sp indicame si tienes una duda y si comprendiste lo que te estoy solicitando.  

# FASE 3 FRONTEND
## RECURSOS HUMANOS (Human Resources)
Empieza a trabajar con el proyecto Frontend (AdventureWorks.Enterprise.App) en este proyecto se van
a estar utilizando los controladores realizados en el proyecto (AdventureWorks.Enterprise.Api)
Te comparto el objetivo del proyecto para que tengas el contexto de lo que se necesita: 
	"Módulos de Negocio
	Para este proyecto, nos enfocaremos en tres esquemas clave de la base de datos AdventureWorks2014:
	-Recursos Humanos (HumanResources): Lógica de negocio sobre empleados y estructura organizacional.
	-Employee, Department, EmployeeDepartmentHistory.
	-Ventas (Sales): Lógica transaccional y relaciones maestro-detalle.
	-SalesOrderHeader, SalesOrderDetail, Customer, SalesPerson.
	-Producción (Production): Gestión de productos, inventario y órdenes de trabajo.
	-Product, ProductInventory, WorkOrder, ProductSubcategory."
	"Scaffolding Inteligente de Controladores: Usa prompts de alto nivel para que Copilot cree los siguientes controladores, asegurando que sigan tus directivas:
	-EmployeesController (CRUD para empleados).
	-OrdersController (Gestión de órdenes de venta).
	-ProductsController (Gestión de productos e inventario)."
Para iniciar enfocate en la parte de Recursos humanos utilizando EmployeeController, 
realiza lo siguiente:
1.	Dentro del proyecto AdventureWorks.Enterprise.App crea una nueva carpeta llamada Models, 
	dentro de ella crea una clase en C# para cada controlador, la clase debe tener las mismas 
	propiedades que el modelo utilizado en el API.
2.	Crea una carpeta Services, dentro de esta carpeta un archivo ApiService.cs que debe contener lo 
	siguiente:
	•	Constructor que inyecte HttpClient
	•	Implementar todos los métodos generados en el controlador.
3.	Configura HttpClient con la ApiKey utilizada en el proyecto AdventureWorks.Enterprise.Api
	• Agrega en el archivo appsettings.json el apikey configurado en el proyecto AdventureWorks.Enterprise.Api
4.	Modifica el archivo Program.cs en el proyecto AdventureWorks.Enterprise.App:
	•	Registra el ApiService y configura el HttpClient leyendo la sección de ApiKeySettings registrada en appsettings.json.
	•	Establece una dirección base que apunte a cada API.
	•	Cada solicitud debe llevar por defecto el DefaultRequestHeader con el valor que se registro en AppSettings.json
5.  Crea los componentes para la funcionalidad de EmployeesController se necesita lo siguiente:
	•	Tabla en donde se visualicen el listado de empleando, mostrando los datos más importantes en columnas
	•	Agrega un botón para crear empleado (Crear Empleado) que direccione a un formulario de creación de empleado.
	•	El formulario de creación de empleado debe de solicitar los campos obligatorios para el método post de EmployeeController
	•	En el formulario de creación deja un botón de cancelar que direccione nuevamente a la página del listado de colaboradores.
6. Para el complemento de Employee permite que al seleccionar uno de los colaboradores redireccione a un formulario en donde se pueda visualizar 
   la información del colaborador, que muestre la misma información que se solicita en el formulario de Crear Empleado. 
	•	Ese nuevo componente debe tener un botón de cancelar que direccione al componente Employee. 
7. En el sidebar crea un acceso directo Para el componente de Employees, el acceso directo se debe llamar (Recursos Humanos)
8. En la página home crea un acceso directo en forma de tarjeta para el componente de Employees que contenga lo siguiente:
	•	Titulo: Empleados
	•	Descripción: Consulta y gestión de empleados.
	•	Botón que direccione al componente de Employees
9. Los componentes deben de ser interactivos y entendibles para el usuario. 
10. Implementa mensajes en pantalla en donde se visualicen mensajes de error (message devuelto en el response del api)
11. Utiliza contributin.md para la estándarización. 
12. Utiliza manejo adecuado de errores.
13. Recuerda que esta aplicación se utilizara en una empresa, por lo que se debe de visualizar de forma adecuada y entendible.
14. Indicame que vas a trabajar, también si tienes duda y no inicies hasta que yo te confirme.
## PORTAL DE PRODUCTOS (Production)
Continuando con el proyecto Frontend (AdventureWorks.Enterprise.App) y
utilizando los controladores realizados en el proyecto (AdventureWorks.Enterprise.Api)
Te comparto de nuevo el objetivo del proyecto para que tengas el contexto de lo que se necesita: 
	"Módulos de Negocio
	Para este proyecto, nos enfocaremos en tres esquemas clave de la base de datos AdventureWorks2014:
	•	Recursos Humanos (HumanResources): Lógica de negocio sobre empleados y estructura organizacional.
	•	Employee, Department, EmployeeDepartmentHistory.
	•	Ventas (Sales): Lógica transaccional y relaciones maestro-detalle.
	•	SalesOrderHeader, SalesOrderDetail, Customer, SalesPerson.
	•	Producción (Production): Gestión de productos, inventario y órdenes de trabajo.
	•	Product, ProductInventory, WorkOrder, ProductSubcategory."
	"Scaffolding Inteligente de Controladores: Usa prompts de alto nivel para que Copilot cree los siguientes controladores, asegurando que sigan tus directivas:
	•	EmployeesController (CRUD para empleados).
	•	OrdersController (Gestión de órdenes de venta).
	•	ProductsController (Gestión de productos e inventario)."
Ahora trabajaremos con la parte de Productos (ProductController, ProductInventoryController,
ProductCategoryController,WorkOrderController):
1.	Utiliza la misma estructura que se trabajo con EmployeeController (Recursos Humanos) para
	la configuración de ProductController, ProductInventoryController,ProductCategoryController,WorkOrderController.
2.  Crea los componentes para la funcionalidad de ProductController se necesita lo siguiente:
	•	Tabla en donde se visualicen el listado de productos, mostrando los datos más importantes en columnas
	•	Agrega una columna adicional al final que contenga un acceso directo que diga "Ver Inventario"
	•	Agrega un botón para crear producto (Crear Producto) que direccione a un formulario de creación de producto.
	•	El formulario de creación de producto debe de solicitar los campos obligatorios para el método post de Product
	•	En el formulario de creación deja un botón de cancelar que direccione nuevamente a la página del listado de productos.
3. Para el complemento de Product permite que al seleccionar "Ver Inventario" uno de los productos redireccione a un formulario en donde se pueda visualizar 
   la información del producto con su inventario, que muestre la misma información que se solicita en el formulario de Crear Producto y un campo adicional
   en donde se vea su inventario.   
	•	Ese nuevo componente debe tener un botón de cancelar que direccione al componente Product. 
4. Crea los componentes para la funcionalidad de ProductInventoryController, se necesita lo siguiente:
	•	Tabla en donde se visualice el listado de productos con su inventario. 
	•	Al seleccionar un producto de la tabla debe direccionar a un formulario (similar al de Crear Producto) 
	    en donde se visualice el Nombre del producto, Inventario y un campo en donde se pueda agregar el inventario actual para
		y un botón "Grabar" que actualice el registro de inventario utilizando el método put de ProductInventoryController, el formulario
	•	En el formulario de actualizar inventario deja un botón de cancelar que direccione nuevamente a la página de inventario.
5. En el sidebar agrega lo siguiente:
	-Producción (Encabezado)
	--Producto (Opción Secundaria): acceso directo Para el componente de Producto
	--Inventario (Opción Secundaria): acceso directo para el componente de Inventario
6. Crea un componente similar a Home, pero llamado HomeProduct que tenga lo siguiente:
	•	Acceso directo en forma de tarjeta para el componente Product:
		Titulo: Productos
		Descripción: Consulta y gestión de productos.
		Botón que direccione al componente de Products
	•	Acceso directo en forma de tarjeta para el componente Inventory:
		Titulo: Inventario
		Descripción: Consulta y actualización de inventarios.
		Botón que direccione al componente de Inventory
7. En la página Home crea un acceso directo en forma de tarjeta para el componente HomeProduct que
    contenga lo siguiente: 
	•	Titulo: Producción
	•	Descripción: Gestión de productos.
	•	Botón que direccione al componente de HomeProduct
8. Los componentes deben de ser interactivos y entendibles para el usuario. 
9. Implementa mensajes en pantalla en donde se visualicen mensajes de error (message devuelto en el response del api)
10. Utiliza contributing.md para la estándarización. 
11. Utiliza manejo adecuado de errores.
12. Recuerda que esta aplicación se utilizara en una empresa, por lo que se debe de visualizar de forma adecuada y entendible.
13. Indicame que vas a trabajar, también si tienes duda y no inicies hasta que yo te confirme.
# FASE 4
## 	GARANTÍA Y CALIDAD DE DEPURACIÓN (Quality and Warranty)
Te comparto el objetivo del proyecto AdventureWorks.Enterprise.Api.tests: 
Fase 4: "Garantía de Calidad y Depuración (Taller 1 y 6)
1. Pruebas Unitarias Conformes: En el proyecto de pruebas, pídele a @workspace que genere tests xUnit para los controladores 
EmployeesController, OrdersController y ProductsController, cubriendo casos de éxito y de fallo.
2. Depuración Asistida por IA: Introduce deliberadamente un bug en uno de los componentes (ej. ProductList.razor). 
Usa el comando /fix para que Copilot identifique, explique y corrija el error."
- Genera pruebas unitarias con xUnit para los controladores EmployeesController, SalesOrderController y ProductsController.
Cubre casos de éxito y de fallo, incluyendo:
	- Creación correcta de entidades.
	- Validación de datos inválidos.
	- Manejo de excepciones.
	- Respuestas esperadas para cada acción (GET, POST, PUT, DELETE).
Incluye ejemplos para cada tipo de caso y usa buenas prácticas en la organización de los tests.