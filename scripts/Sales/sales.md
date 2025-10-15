 #  Sales
 ## Sales.SalesOrderHeader (Contiene el encabezado de cada orden de venta)
	CREATE TABLE [Sales].[SalesOrderHeader](
		[SalesOrderID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[RevisionNumber] [tinyint] NOT NULL,
		[OrderDate] [datetime] NOT NULL,
		[DueDate] [datetime] NOT NULL,
		[ShipDate] [datetime] NULL,
		[Status] [tinyint] NOT NULL,
		[OnlineOrderFlag] [dbo].[Flag] NOT NULL,
		[SalesOrderNumber]  AS (isnull(N'SO'+CONVERT([nvarchar](23),[SalesOrderID]),N'*** ERROR ***')),
		[PurchaseOrderNumber] [dbo].[OrderNumber] NULL,
		[AccountNumber] [dbo].[AccountNumber] NULL,
		[CustomerID] [int] NOT NULL,
		[SalesPersonID] [int] NULL,
		[TerritoryID] [int] NULL,
		[BillToAddressID] [int] NOT NULL,
		[ShipToAddressID] [int] NOT NULL,
		[ShipMethodID] [int] NOT NULL,
		[CreditCardID] [int] NULL,
		[CreditCardApprovalCode] [varchar](15) NULL,
		[CurrencyRateID] [int] NULL,
		[SubTotal] [money] NOT NULL,
		[TaxAmt] [money] NOT NULL,
		[Freight] [money] NOT NULL,
		[TotalDue]  AS (isnull(([SubTotal]+[TaxAmt])+[Freight],(0))),
		[Comment] [nvarchar](128) NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_SalesOrderHeader_SalesOrderID] PRIMARY KEY CLUSTERED 
	(
		[SalesOrderID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
## Sales.SalesOrderDetail (Contiene el detalle de cada orden de venta): 
	CREATE TABLE [Sales].[SalesOrderDetail](
		[SalesOrderID] [int] NOT NULL,
		[SalesOrderDetailID] [int] IDENTITY(1,1) NOT NULL,
		[CarrierTrackingNumber] [nvarchar](25) NULL,
		[OrderQty] [smallint] NOT NULL,
		[ProductID] [int] NOT NULL,
		[SpecialOfferID] [int] NOT NULL,
		[UnitPrice] [money] NOT NULL,
		[UnitPriceDiscount] [money] NOT NULL,
		[LineTotal]  AS (isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0))),
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_SalesOrderDetail_SalesOrderID_SalesOrderDetailID] PRIMARY KEY CLUSTERED 
	(
		[SalesOrderID] ASC,
		[SalesOrderDetailID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
## Sales.Customer (Contiene los datos de los clientes):
	CREATE TABLE [Sales].[Customer](
		[CustomerID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[PersonID] [int] NULL,
		[StoreID] [int] NULL,
		[TerritoryID] [int] NULL,
		[AccountNumber]  AS (isnull('AW'+[dbo].[ufnLeadingZeros]([CustomerID]),'')),
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_Customer_CustomerID] PRIMARY KEY CLUSTERED 
	(
		[CustomerID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
## Sales.SalesPerson (Contiene los datos del vendedor):
	CREATE TABLE [Sales].[SalesPerson](
		[BusinessEntityID] [int] NOT NULL,
		[TerritoryID] [int] NULL,
		[SalesQuota] [money] NULL,
		[Bonus] [money] NOT NULL,
		[CommissionPct] [smallmoney] NOT NULL,
		[SalesYTD] [money] NOT NULL,
		[SalesLastYear] [money] NOT NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_SalesPerson_BusinessEntityID] PRIMARY KEY CLUSTERED 
	(
		[BusinessEntityID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
## Production.Product (Contiene los datos de productos):
	CREATE TABLE [Production].[Product](
		[ProductID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [dbo].[Name] NOT NULL,
		[ProductNumber] [nvarchar](25) NOT NULL,
		[MakeFlag] [dbo].[Flag] NOT NULL,
		[FinishedGoodsFlag] [dbo].[Flag] NOT NULL,
		[Color] [nvarchar](15) NULL,
		[SafetyStockLevel] [smallint] NOT NULL,
		[ReorderPoint] [smallint] NOT NULL,
		[StandardCost] [money] NOT NULL,
		[ListPrice] [money] NOT NULL,
		[Size] [nvarchar](5) NULL,
		[SizeUnitMeasureCode] [nchar](3) NULL,
		[WeightUnitMeasureCode] [nchar](3) NULL,
		[Weight] [decimal](8, 2) NULL,
		[DaysToManufacture] [int] NOT NULL,
		[ProductLine] [nchar](2) NULL,
		[Class] [nchar](2) NULL,
		[Style] [nchar](2) NULL,
		[ProductSubcategoryID] [int] NULL,
		[ProductModelID] [int] NULL,
		[SellStartDate] [datetime] NOT NULL,
		[SellEndDate] [datetime] NULL,
		[DiscontinuedDate] [datetime] NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_Product_ProductID] PRIMARY KEY CLUSTERED 
	(
		[ProductID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO
## Production.ProductCategory (Contiene las categorías de productos):
		CREATE TABLE [Production].[ProductCategory](
		[ProductCategoryID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [dbo].[Name] NOT NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_ProductCategory_ProductCategoryID] PRIMARY KEY CLUSTERED 
	(
		[ProductCategoryID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO

## Production.ProductSubcategory (Contiene las subcategorías de productos):
	CREATE TABLE [Production].[ProductSubcategory](
		[ProductSubcategoryID] [int] IDENTITY(1,1) NOT NULL,
		[ProductCategoryID] [int] NOT NULL,
		[Name] [dbo].[Name] NOT NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_ProductSubcategory_ProductSubcategoryID] PRIMARY KEY CLUSTERED 
	(
		[ProductSubcategoryID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO