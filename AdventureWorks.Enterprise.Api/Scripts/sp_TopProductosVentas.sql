USE [AdventureWorks2014]
GO

-- =============================================
-- Autor:      Copilot (actualizado)
-- Fecha:      2023 / Actualizado
-- Descripcion: Stored Procedure para obtener el top 10 de productos más vendidos
--              (column aliases alineados al frontend)
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_TopProductosVentas]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT TOP 10
            p.ProductID AS ProductID,
            p.Name AS ProductName,
            AVG(sod.UnitPrice) AS UnitPrice,
            SUM(sod.OrderQty) AS TotalQuantitySold,
            SUM(sod.LineTotal) AS TotalSalesAmount,
            pc.Name AS CategoryName,
            psc.Name AS SubCategoryName
        FROM Production.Product p
        INNER JOIN Sales.SalesOrderDetail sod ON p.ProductID = sod.ProductID
        LEFT JOIN Production.ProductSubcategory psc ON p.ProductSubcategoryID = psc.ProductSubcategoryID
        LEFT JOIN Production.ProductCategory pc ON psc.ProductCategoryID = pc.ProductCategoryID
        GROUP BY p.ProductID, p.Name, pc.Name, psc.Name
        ORDER BY SUM(sod.LineTotal) DESC;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
GO