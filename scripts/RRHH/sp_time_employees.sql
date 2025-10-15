-- =============================================
-- Autor: Sistema AdventureWorks
-- Fecha: 2024
-- Descripción: Obtiene los empleados con más tiempo en su departamento actual
-- Parámetros: @IntCantidad - Número de empleados a devolver (TOP N)
-- =============================================

IF OBJECT_ID('HumanResources.sp_DepartamentoEmpleado', 'P') IS NOT NULL
    DROP PROCEDURE HumanResources.sp_DepartamentoEmpleado;
GO

CREATE PROCEDURE HumanResources.sp_DepartamentoEmpleado
    @IntCantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validar parámetro de entrada
        IF @IntCantidad IS NULL OR @IntCantidad <= 0
        BEGIN
            RAISERROR('El parámetro @IntCantidad debe ser mayor que 0', 16, 1);
            RETURN;
        END
        
        -- Obtener empleados con más tiempo en su departamento actual
        SELECT TOP (@IntCantidad)
            e.BusinessEntityID AS IntBusinessEntityID,
            CONCAT(p.FirstName, ' ', ISNULL(p.MiddleName + ' ', ''), p.LastName) AS StrFullName,
            p.FirstName AS StrFirstName,
            p.MiddleName AS StrMiddleName,
            p.LastName AS StrLastName,
            d.Name AS StrDepartmentName,
            d.DepartmentID AS IntDepartmentID,
            edh.StartDate AS DtmStartDate,
            DATEDIFF(DAY, edh.StartDate, GETDATE()) AS IntDaysInDepartment,
            DATEDIFF(YEAR, edh.StartDate, GETDATE()) AS IntYearsInDepartment,
            e.JobTitle AS StrJobTitle,
            e.HireDate AS DtmHireDate
        FROM HumanResources.Employee e
        INNER JOIN Person.Person p ON e.BusinessEntityID = p.BusinessEntityID
        INNER JOIN HumanResources.EmployeeDepartmentHistory edh ON e.BusinessEntityID = edh.BusinessEntityID
        INNER JOIN HumanResources.Department d ON edh.DepartmentID = d.DepartmentID
        WHERE edh.EndDate IS NULL  -- Departamento actual (sin fecha de fin)
          AND e.CurrentFlag = 1    -- Empleados activos
        ORDER BY IntDaysInDepartment DESC; -- Ordenar por más tiempo en el departamento
        
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO