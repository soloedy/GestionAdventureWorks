-- Script de creación del stored procedure sp_BajoInventario
CREATE PROCEDURE [Production].[sp_BajoInventario]
    @CantidadLimite INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT
            p.ProductID,
            p.Name AS ProductName,
            p.ProductNumber,
            pi.Quantity,
            l.Name AS LocationName,
            ps.Name AS SubcategoryName,
            pc.Name AS CategoryName
        FROM Production.ProductInventory pi
        INNER JOIN Production.Product p ON pi.ProductID = p.ProductID
        INNER JOIN Production.Location l ON pi.LocationID = l.LocationID
        LEFT JOIN Production.ProductSubcategory ps ON p.ProductSubcategoryID = ps.ProductSubcategoryID
        LEFT JOIN Production.ProductCategory pc ON ps.ProductCategoryID = pc.ProductCategoryID
        WHERE pi.Quantity <= @CantidadLimite
        ORDER BY pi.Quantity ASC, p.Name
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Error en sp_BajoInventario: %s', 16, 1, @ErrorMessage);
    END CATCH
END
GO
