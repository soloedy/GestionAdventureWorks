using Microsoft.EntityFrameworkCore;
using AdventureWorks.Enterprise.Api.Entities;
using AdventureWorks.Enterprise.Api.DTOs;

namespace AdventureWorks.Enterprise.Api.Data
{
    public class AdventureWorksDbContext : DbContext
    {
        public AdventureWorksDbContext(DbContextOptions<AdventureWorksDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesPerson> SalesPersons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet<UnitMeasure> UnitMeasures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Employee existente
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee", "HumanResources");
                entity.HasKey(e => e.BusinessEntityID);

                entity.Property(e => e.BusinessEntityID).ValueGeneratedNever();
                entity.Property(e => e.NationalIDNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.LoginID).IsRequired().HasMaxLength(256);
                entity.Property(e => e.JobTitle).IsRequired().HasMaxLength(50);
                entity.Property(e => e.BirthDate).HasColumnType("date");
                entity.Property(e => e.MaritalStatus).IsRequired().HasMaxLength(1).IsFixedLength();
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(1).IsFixedLength();
                entity.Property(e => e.HireDate).HasColumnType("date");
                entity.Property(e => e.SalariedFlag).HasColumnType("bit");
                entity.Property(e => e.VacationHours).HasColumnType("smallint");
                entity.Property(e => e.SickLeaveHours).HasColumnType("smallint");
                entity.Property(e => e.CurrentFlag).HasColumnType("bit");
                entity.Property(e => e.RowGuid).HasColumnName("rowguid");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                // OrganizationNode hierarchyid is not natively supported by EF Core; map as string/binary if needed.
                entity.Property(e => e.OrganizationNode).HasColumnType("hierarchyid");
                entity.Property(e => e.OrganizationLevel).HasColumnType("int").ValueGeneratedOnAddOrUpdate();
            });

            // Configuración de SalesOrderHeader
            modelBuilder.Entity<SalesOrderHeader>(entity =>
            {
                entity.ToTable("SalesOrderHeader", "Sales");
                entity.HasKey(e => e.SalesOrderID);

                entity.Property(e => e.SalesOrderID).HasColumnType("int").ValueGeneratedOnAdd();
                entity.Property(e => e.RevisionNumber).HasColumnType("tinyint").IsRequired();
                entity.Property(e => e.OrderDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.DueDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ShipDate).HasColumnType("datetime");
                entity.Property(e => e.Status).HasColumnType("tinyint").IsRequired();
                entity.Property(e => e.OnlineOrderFlag).HasColumnType("bit").IsRequired();
                entity.Property(e => e.SalesOrderNumber).HasComputedColumnSql("isnull(N'SO'+CONVERT([nvarchar](23),[SalesOrderID]),N'*** ERROR ***')");
                entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(25);
                entity.Property(e => e.AccountNumber).HasMaxLength(15);
                entity.Property(e => e.CustomerID).IsRequired();
                entity.Property(e => e.SalesPersonID);
                entity.Property(e => e.TerritoryID);
                entity.Property(e => e.BillToAddressID).IsRequired();
                entity.Property(e => e.ShipToAddressID).IsRequired();
                entity.Property(e => e.ShipMethodID).IsRequired();
                entity.Property(e => e.CreditCardID);
                entity.Property(e => e.CreditCardApprovalCode).HasMaxLength(15);
                entity.Property(e => e.CurrencyRateID);
                entity.Property(e => e.SubTotal).HasColumnType("money").IsRequired();
                entity.Property(e => e.TaxAmt).HasColumnType("money").IsRequired();
                entity.Property(e => e.Freight).HasColumnType("money").IsRequired();
                entity.Property(e => e.TotalDue).HasComputedColumnSql("isnull(([SubTotal]+[TaxAmt])+[Freight],(0))");
                entity.Property(e => e.Comment).HasMaxLength(128);
                entity.Property(e => e.RowGuid).HasColumnName("rowguid").IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
                
                // Relaciones
                entity.HasMany(e => e.SalesOrderDetails)
                      .WithOne(e => e.SalesOrderHeader)
                      .HasForeignKey(e => e.SalesOrderID);

                entity.HasOne(e => e.Customer)
                      .WithMany(e => e.SalesOrderHeaders)
                      .HasForeignKey(e => e.CustomerID);

                entity.HasOne(e => e.SalesPerson)
                      .WithMany(e => e.SalesOrderHeaders)
                      .HasForeignKey(e => e.SalesPersonID)
                      .IsRequired(false);
            });

            // Configuración de SalesOrderDetail
            modelBuilder.Entity<SalesOrderDetail>(entity =>
            {
                entity.ToTable("SalesOrderDetail", "Sales");
                entity.HasKey(e => new { e.SalesOrderID, e.SalesOrderDetailID });

                entity.Property(e => e.SalesOrderID).IsRequired();
                entity.Property(e => e.SalesOrderDetailID).HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
                entity.Property(e => e.CarrierTrackingNumber).HasMaxLength(25);
                entity.Property(e => e.OrderQty).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.ProductID).IsRequired();
                entity.Property(e => e.SpecialOfferID).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("money").IsRequired();
                entity.Property(e => e.UnitPriceDiscount).HasColumnType("money").IsRequired();
                entity.Property(e => e.LineTotal).HasComputedColumnSql("isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0))");
                entity.Property(e => e.RowGuid).HasColumnName("rowguid").IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
                
                // Relaciones
                entity.HasOne(e => e.Product)
                      .WithMany(e => e.SalesOrderDetails)
                      .HasForeignKey(e => e.ProductID);
            });

            // Configuración de Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Sales");
                entity.HasKey(e => e.CustomerID);

                entity.Property(e => e.CustomerID).HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
                entity.Property(e => e.PersonID);
                entity.Property(e => e.StoreID);
                entity.Property(e => e.TerritoryID);
                entity.Property(e => e.AccountNumber).HasComputedColumnSql("isnull('AW'+[dbo].[ufnLeadingZeros]([CustomerID]),'')");
                entity.Property(e => e.RowGuid).HasColumnName("rowguid").IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
            });

            // Configuración de SalesPerson
            modelBuilder.Entity<SalesPerson>(entity =>
            {
                entity.ToTable("SalesPerson", "Sales");
                entity.HasKey(e => e.BusinessEntityID);

                entity.Property(e => e.BusinessEntityID).ValueGeneratedNever().IsRequired();
                entity.Property(e => e.TerritoryID);
                entity.Property(e => e.SalesQuota).HasColumnType("money");
                entity.Property(e => e.Bonus).HasColumnType("money").IsRequired();
                entity.Property(e => e.CommissionPct).HasColumnType("smallmoney").IsRequired();
                entity.Property(e => e.SalesYTD).HasColumnType("money").IsRequired();
                entity.Property(e => e.SalesLastYear).HasColumnType("money").IsRequired();
                entity.Property(e => e.RowGuid).HasColumnName("rowguid").IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
            });

            // Configuración de Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Production");
                entity.HasKey(e => e.ProductID);

                entity.Property(e => e.ProductID).HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ProductNumber).HasMaxLength(25).IsRequired();
                entity.Property(e => e.MakeFlag).HasColumnType("bit").IsRequired();
                entity.Property(e => e.FinishedGoodsFlag).HasColumnType("bit").IsRequired();
                entity.Property(e => e.Color).HasMaxLength(15);
                entity.Property(e => e.SafetyStockLevel).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.ReorderPoint).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.StandardCost).HasColumnType("money").IsRequired();
                entity.Property(e => e.ListPrice).HasColumnType("money").IsRequired();
                entity.Property(e => e.Size).HasMaxLength(5);
                entity.Property(e => e.SizeUnitMeasureCode).HasMaxLength(3).IsFixedLength();
                entity.Property(e => e.WeightUnitMeasureCode).HasMaxLength(3).IsFixedLength();
                entity.Property(e => e.Weight).HasColumnType("decimal(8, 2)");
                entity.Property(e => e.DaysToManufacture).IsRequired();
                entity.Property(e => e.ProductLine).HasMaxLength(2).IsFixedLength();
                entity.Property(e => e.Class).HasMaxLength(2).IsFixedLength();
                entity.Property(e => e.Style).HasMaxLength(2).IsFixedLength();
                entity.Property(e => e.ProductSubcategoryID);
                entity.Property(e => e.ProductModelID);
                entity.Property(e => e.SellStartDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.SellEndDate).HasColumnType("datetime");
                entity.Property(e => e.DiscontinuedDate).HasColumnType("datetime");
                entity.Property(e => e.RowGuid).HasColumnName("rowguid").IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
                
                // Relación con ProductSubcategory
                entity.HasOne(e => e.ProductSubcategory)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.ProductSubcategoryID)
                      .IsRequired(false);
            });
            
            // Configuración de ProductInventory
            modelBuilder.Entity<ProductInventory>(entity =>
            {
                entity.ToTable("ProductInventory", "Production");
                entity.HasKey(e => new { e.ProductID, e.LocationID });

                entity.Property(e => e.ProductID).IsRequired();
                entity.Property(e => e.LocationID).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.Shelf).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Bin).HasColumnType("tinyint").IsRequired();
                entity.Property(e => e.Quantity).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.RowGuid).HasColumnName("rowguid").IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
                
                // Relaciones
                entity.HasOne(e => e.Product)
                      .WithMany(p => p.ProductInventories)
                      .HasForeignKey(e => e.ProductID);
            });
            
            // Configuración de WorkOrder
            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.ToTable("WorkOrder", "Production");
                entity.HasKey(e => e.WorkOrderID);

                entity.Property(e => e.WorkOrderID).HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
                entity.Property(e => e.ProductID).IsRequired();
                entity.Property(e => e.OrderQty).IsRequired();
                entity.Property(e => e.StockedQty).HasComputedColumnSql("isnull([OrderQty]-[ScrappedQty],(0))");
                entity.Property(e => e.ScrappedQty).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.StartDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.DueDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ScrapReasonID).HasColumnType("smallint");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
                
                // Relaciones
                entity.HasOne(e => e.Product)
                      .WithMany(p => p.WorkOrders)
                      .HasForeignKey(e => e.ProductID);
            });
            
            // Configuración de ProductSubcategory
            modelBuilder.Entity<ProductSubcategory>(entity =>
            {
                entity.ToTable("ProductSubcategory", "Production");
                entity.HasKey(e => e.ProductSubcategoryID);

                entity.Property(e => e.ProductSubcategoryID).HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
                entity.Property(e => e.ProductCategoryID).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.RowGuid).HasColumnName("rowguid").IsRequired();
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired();
            });

            // Configuración de UnitMeasure
            modelBuilder.Entity<UnitMeasure>(entity =>
            {
                entity.HasKey(e => e.UnitMeasureCode);
                entity.ToTable("UnitMeasure", "Production");
                entity.Property(e => e.UnitMeasureCode).HasMaxLength(3).IsFixedLength();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ModifiedDate).IsRequired();
            });

            // Configurar EmployeeFullReadDto como entidad sin clave para consultas SQL raw
            modelBuilder.Entity<EmployeeFullReadDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // No mapea a una tabla específica, se usa solo para consultas SQL raw
            });

            // Configurar EmployeeDepartmentDto como entidad sin clave para consultas SQL raw
            modelBuilder.Entity<EmployeeDepartmentDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // No mapea a una tabla específica, se usa solo para consultas SQL raw
            });

            // Configurar TopProductDto como entidad sin clave para consultas SQL raw
            modelBuilder.Entity<TopProductDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // No mapea a una tabla específica, se usa solo para consultas SQL raw
            });

            // Configurar ProductLowInventoryDto como entidad sin clave para SP de bajo inventario
            modelBuilder.Entity<ProductLowInventoryDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // No mapea a una tabla específica, se usa solo para consultas SQL raw
            });
        }
    }
}
