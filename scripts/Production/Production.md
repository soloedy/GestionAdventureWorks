# production

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

## Production.ProductInventory (Contiene el inventario de productos):

	CREATE TABLE [Production].[ProductInventory](
		[ProductID] [int] NOT NULL,
		[LocationID] [smallint] NOT NULL,
		[Shelf] [nvarchar](10) NOT NULL,
		[Bin] [tinyint] NOT NULL,
		[Quantity] [smallint] NOT NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_ProductInventory_ProductID_LocationID] PRIMARY KEY CLUSTERED 
	(
		[ProductID] ASC,
		[LocationID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO

## Production.WorkOrder (Contiene las órdenes de trabajo para la producción de productos):

	CREATE TABLE [Production].[WorkOrder](
		[WorkOrderID] [int] IDENTITY(1,1) NOT NULL,
		[ProductID] [int] NOT NULL,
		[OrderQty] [int] NOT NULL,
		[StockedQty]  AS (isnull([OrderQty]-[ScrappedQty],(0))),
		[ScrappedQty] [smallint] NOT NULL,
		[StartDate] [datetime] NOT NULL,
		[EndDate] [datetime] NULL,
		[DueDate] [datetime] NOT NULL,
		[ScrapReasonID] [smallint] NULL,
		[ModifiedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_WorkOrder_WorkOrderID] PRIMARY KEY CLUSTERED 
	(
		[WorkOrderID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	GO