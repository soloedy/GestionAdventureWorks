USE [AdventureWorks2014]
GO

-- =============================================
-- Autor:      Copilot
-- Fecha:      2023
-- Descripcion: Stored Procedure para crear órdenes de venta con su cabecera y detalles
-- =============================================
CREATE OR ALTER PROCEDURE [Sales].[sp_CreacionOrdenes]
    -- Parámetros para la cabecera
    @RevisionNumber TINYINT,
    @OrderDate DATETIME,
    @DueDate DATETIME,
    @ShipDate DATETIME = NULL,
    @Status TINYINT,
    @OnlineOrderFlag BIT,
    @PurchaseOrderNumber NVARCHAR(25) = NULL,
    @CustomerID INT,
    @SalesPersonID INT = NULL,
    @TerritoryID INT = NULL,
    @BillToAddressID INT,
    @ShipToAddressID INT,
    @ShipMethodID INT,
    @CreditCardID INT = NULL,
    @CreditCardApprovalCode VARCHAR(15) = NULL,
    @CurrencyRateID INT = NULL,
    @SubTotal MONEY,
    @TaxAmt MONEY,
    @Freight MONEY,
    @Comment NVARCHAR(128) = NULL,
    -- Parámetro de tabla para los detalles
    @OrderDetails [Sales].[SalesOrderDetailType] READONLY,
    -- Parámetro de salida
    @SalesOrderID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Variables para control de errores
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    
    -- Variable para GUID único
    DECLARE @RowGuid UNIQUEIDENTIFIER = NEWID();
    DECLARE @ModifiedDate DATETIME = GETDATE();
    
    -- Iniciar transacción
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validaciones previas
        IF NOT EXISTS (SELECT 1 FROM [Sales].[Customer] WHERE CustomerID = @CustomerID)
        BEGIN
            RAISERROR('El cliente especificado no existe.', 16, 1);
        END
        
        IF @SalesPersonID IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [Sales].[SalesPerson] WHERE BusinessEntityID = @SalesPersonID)
        BEGIN
            RAISERROR('El vendedor especificado no existe.', 16, 1);
        END
        
        -- Insertar cabecera de la orden
        INSERT INTO [Sales].[SalesOrderHeader]
        (
            [RevisionNumber],
            [OrderDate],
            [DueDate],
            [ShipDate],
            [Status],
            [OnlineOrderFlag],
            [PurchaseOrderNumber],
            [CustomerID],
            [SalesPersonID],
            [TerritoryID],
            [BillToAddressID],
            [ShipToAddressID],
            [ShipMethodID],
            [CreditCardID],
            [CreditCardApprovalCode],
            [CurrencyRateID],
            [SubTotal],
            [TaxAmt],
            [Freight],
            [Comment],
            [rowguid],
            [ModifiedDate]
        )
        VALUES
        (
            @RevisionNumber,
            @OrderDate,
            @DueDate,
            @ShipDate,
            @Status,
            @OnlineOrderFlag,
            @PurchaseOrderNumber,
            @CustomerID,
            @SalesPersonID,
            @TerritoryID,
            @BillToAddressID,
            @ShipToAddressID,
            @ShipMethodID,
            @CreditCardID,
            @CreditCardApprovalCode,
            @CurrencyRateID,
            @SubTotal,
            @TaxAmt,
            @Freight,
            @Comment,
            @RowGuid,
            @ModifiedDate
        );
        
        -- Obtener el ID de la orden recién creada
        SET @SalesOrderID = SCOPE_IDENTITY();
        
        -- Insertar los detalles de la orden
        INSERT INTO [Sales].[SalesOrderDetail]
        (
            [SalesOrderID],
            [CarrierTrackingNumber],
            [OrderQty],
            [ProductID],
            [SpecialOfferID],
            [UnitPrice],
            [UnitPriceDiscount],
            [rowguid],
            [ModifiedDate]
        )
        SELECT
            @SalesOrderID,
            [CarrierTrackingNumber],
            [OrderQty],
            [ProductID],
            [SpecialOfferID],
            [UnitPrice],
            [UnitPriceDiscount],
            NEWID(), -- Generar un nuevo GUID para cada detalle
            @ModifiedDate
        FROM @OrderDetails;
        
        -- Si todo está bien, confirmar la transacción
        COMMIT TRANSACTION;
        
        -- Retornar información sobre la orden creada
        SELECT @SalesOrderID AS SalesOrderID, 'Orden creada exitosamente' AS Message;
        
    END TRY
    BEGIN CATCH
        -- Si hay un error, hacer rollback de la transacción
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        -- Capturar detalles del error
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        -- Relanzar el error con información adicional
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        
        -- Retornar -1 como indicador de error
        SET @SalesOrderID = -1;
    END CATCH;
END;
GO

-- Crear el tipo de tabla para los detalles de la orden
IF NOT EXISTS (SELECT * FROM sys.types WHERE name = 'SalesOrderDetailType' AND schema_id = SCHEMA_ID('Sales'))
BEGIN
    CREATE TYPE [Sales].[SalesOrderDetailType] AS TABLE
    (
        [CarrierTrackingNumber] NVARCHAR(25) NULL,
        [OrderQty] SMALLINT NOT NULL,
        [ProductID] INT NOT NULL,
        [SpecialOfferID] INT NOT NULL,
        [UnitPrice] MONEY NOT NULL,
        [UnitPriceDiscount] MONEY NOT NULL
    );
END
GO