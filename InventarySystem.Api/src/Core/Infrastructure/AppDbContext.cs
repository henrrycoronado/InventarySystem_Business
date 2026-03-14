using System;
using System.Collections.Generic;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.src.Core.Infrastructure;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Batch> Batches { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyAttribute> CompanyAttributes { get; set; }

    public virtual DbSet<CompanyProduct> CompanyProducts { get; set; }

    public virtual DbSet<CompanySku> CompanySkus { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<GlobalCategory> GlobalCategories { get; set; }

    public virtual DbSet<GlobalProduct> GlobalProducts { get; set; }

    public virtual DbSet<Kardex> Kardices { get; set; }

    public virtual DbSet<Movement> Movements { get; set; }

    public virtual DbSet<MovementDetail> MovementDetails { get; set; }

    public virtual DbSet<MovementStatus> MovementStatuses { get; set; }

    public virtual DbSet<MovementType> MovementTypes { get; set; }

    public virtual DbSet<PdvItemStatus> PdvItemStatuses { get; set; }

    public virtual DbSet<PdvMenu> PdvMenus { get; set; }

    public virtual DbSet<PdvMenuItem> PdvMenuItems { get; set; }

    public virtual DbSet<PdvOrder> PdvOrders { get; set; }

    public virtual DbSet<PdvOrderDetail> PdvOrderDetails { get; set; }

    public virtual DbSet<PdvOrderStatus> PdvOrderStatuses { get; set; }

    public virtual DbSet<PdvStation> PdvStations { get; set; }

    public virtual DbSet<PdvStationCategory> PdvStationCategories { get; set; }

    public virtual DbSet<PdvTable> PdvTables { get; set; }

    public virtual DbSet<PdvWaiter> PdvWaiters { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<SaleStatus> SaleStatuses { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<SkuAttributeValue> SkuAttributeValues { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Batch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("batches_pkey");

            entity.ToTable("batches", "inventory");

            entity.HasIndex(e => new { e.SkuId, e.BatchNumber }, "batches_sku_id_batch_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchNumber)
                .HasMaxLength(50)
                .HasColumnName("batch_number");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ManufactureDate).HasColumnName("manufacture_date");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");

            entity.HasOne(d => d.Sku).WithMany(p => p.Batches)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("batches_sku_id_fkey");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companies_pkey");

            entity.ToTable("companies", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CompanyAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_attributes_pkey");

            entity.ToTable("company_attributes", "inventory");

            entity.HasIndex(e => new { e.CompanyId, e.Name }, "company_attributes_company_id_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyAttributes)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_attributes_company_id_fkey");
        });

        modelBuilder.Entity<CompanyProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_products_pkey");

            entity.ToTable("company_products", "inventory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.GlobalProductId).HasColumnName("global_product_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LocalNameAlias)
                .HasMaxLength(150)
                .HasColumnName("local_name_alias");
            entity.Property(e => e.WholesalePrice)
                .HasPrecision(10, 2)
                .HasDefaultValue(0.00m)
                .HasColumnName("wholesale_price");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyProducts)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_products_company_id_fkey");

            entity.HasOne(d => d.GlobalProduct).WithMany(p => p.CompanyProducts)
                .HasForeignKey(d => d.GlobalProductId)
                .HasConstraintName("company_products_global_product_id_fkey");
        });

        modelBuilder.Entity<CompanySku>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_skus_pkey");

            entity.ToTable("company_skus", "inventory");

            entity.HasIndex(e => new { e.CompanyProductId, e.InternalSku }, "company_skus_company_product_id_internal_sku_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyProductId).HasColumnName("company_product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.InternalSku)
                .HasMaxLength(50)
                .HasColumnName("internal_sku");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.RetailPrice)
                .HasPrecision(10, 2)
                .HasDefaultValue(0.00m)
                .HasColumnName("retail_price");

            entity.HasOne(d => d.CompanyProduct).WithMany(p => p.CompanySkus)
                .HasForeignKey(d => d.CompanyProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_skus_company_product_id_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customers_pkey");

            entity.ToTable("customers", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.Company).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customers_company_id_fkey");
        });

        modelBuilder.Entity<GlobalCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("global_categories_pkey");

            entity.ToTable("global_categories", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<GlobalProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("global_products_pkey");

            entity.ToTable("global_products", "shared");

            entity.HasIndex(e => e.UpcBarcode, "global_products_upc_barcode_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .HasColumnName("brand");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.UpcBarcode)
                .HasMaxLength(50)
                .HasColumnName("upc_barcode");

            entity.HasOne(d => d.Category).WithMany(p => p.GlobalProducts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("global_products_category_id_fkey");
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kardex_pkey");

            entity.ToTable("kardex", "inventory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BalanceAfter)
                .HasPrecision(10, 2)
                .HasColumnName("balance_after");
            entity.Property(e => e.BatchId).HasColumnName("batch_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MovementDetailId).HasColumnName("movement_detail_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Batch).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.BatchId)
                .HasConstraintName("kardex_batch_id_fkey");

            entity.HasOne(d => d.Company).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_company_id_fkey");

            entity.HasOne(d => d.MovementDetail).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.MovementDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_movement_detail_id_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_sku_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_type_id_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_warehouse_id_fkey");
        });

        modelBuilder.Entity<Movement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movements_pkey");

            entity.ToTable("movements", "inventory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MovementDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("movement_date");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TargetWarehouseId).HasColumnName("target_warehouse_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Company).WithMany(p => p.Movements)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movements_company_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Movements)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movements_status_id_fkey");

            entity.HasOne(d => d.TargetWarehouse).WithMany(p => p.MovementTargetWarehouses)
                .HasForeignKey(d => d.TargetWarehouseId)
                .HasConstraintName("movements_target_warehouse_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Movements)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movements_type_id_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.MovementWarehouses)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movements_warehouse_id_fkey");
        });

        modelBuilder.Entity<MovementDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movement_details_pkey");

            entity.ToTable("movement_details", "inventory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchId).HasColumnName("batch_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MovementId).HasColumnName("movement_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.UnitCost)
                .HasPrecision(10, 2)
                .HasColumnName("unit_cost");

            entity.HasOne(d => d.Batch).WithMany(p => p.MovementDetails)
                .HasForeignKey(d => d.BatchId)
                .HasConstraintName("movement_details_batch_id_fkey");

            entity.HasOne(d => d.Movement).WithMany(p => p.MovementDetails)
                .HasForeignKey(d => d.MovementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movement_details_movement_id_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.MovementDetails)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movement_details_sku_id_fkey");
        });

        modelBuilder.Entity<MovementStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movement_statuses_pkey");

            entity.ToTable("movement_statuses", "inventory");

            entity.HasIndex(e => e.Code, "movement_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MovementType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movement_types_pkey");

            entity.ToTable("movement_types", "inventory");

            entity.HasIndex(e => e.Code, "movement_types_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Operation)
                .HasMaxLength(1)
                .HasColumnName("operation");
        });

        modelBuilder.Entity<PdvItemStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_item_statuses_pkey");

            entity.ToTable("pdv_item_statuses", "sales");

            entity.HasIndex(e => e.Code, "pdv_item_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PdvMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_menus_pkey");

            entity.ToTable("pdv_menus", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Company).WithMany(p => p.PdvMenus)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_menus_company_id_fkey");
        });

        modelBuilder.Entity<PdvMenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_menu_items_pkey");

            entity.ToTable("pdv_menu_items", "sales");

            entity.HasIndex(e => new { e.MenuId, e.SkuId }, "pdv_menu_items_menu_id_sku_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.StationId).HasColumnName("station_id");

            entity.HasOne(d => d.Menu).WithMany(p => p.PdvMenuItems)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_menu_items_menu_id_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.PdvMenuItems)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_menu_items_sku_id_fkey");

            entity.HasOne(d => d.Station).WithMany(p => p.PdvMenuItems)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("pdv_menu_items_station_id_fkey");
        });

        modelBuilder.Entity<PdvOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_orders_pkey");

            entity.ToTable("pdv_orders", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClosedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("closed_at");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.OpenedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("opened_at");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TableId).HasColumnName("table_id");
            entity.Property(e => e.WaiterId).HasColumnName("waiter_id");

            entity.HasOne(d => d.Company).WithMany(p => p.PdvOrders)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_orders_company_id_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.PdvOrders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("pdv_orders_customer_id_fkey");

            entity.HasOne(d => d.Sale).WithMany(p => p.PdvOrders)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("pdv_orders_sale_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.PdvOrders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_orders_status_id_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.PdvOrders)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_orders_table_id_fkey");

            entity.HasOne(d => d.Waiter).WithMany(p => p.PdvOrders)
                .HasForeignKey(d => d.WaiterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_orders_waiter_id_fkey");
        });

        modelBuilder.Entity<PdvOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_order_details_pkey");

            entity.ToTable("pdv_order_details", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MenuItemId).HasColumnName("menu_item_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.PdvOrderDetails)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_order_details_menu_item_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.PdvOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_order_details_order_id_fkey");

            entity.HasOne(d => d.Station).WithMany(p => p.PdvOrderDetails)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("pdv_order_details_station_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.PdvOrderDetails)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_order_details_status_id_fkey");
        });

        modelBuilder.Entity<PdvOrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_order_statuses_pkey");

            entity.ToTable("pdv_order_statuses", "sales");

            entity.HasIndex(e => e.Code, "pdv_order_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PdvStation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_stations_pkey");

            entity.ToTable("pdv_stations", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Company).WithMany(p => p.PdvStations)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_stations_company_id_fkey");
        });

        modelBuilder.Entity<PdvStationCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_station_categories_pkey");

            entity.ToTable("pdv_station_categories", "sales");

            entity.HasIndex(e => new { e.StationId, e.GlobalCategoryId }, "pdv_station_categories_station_id_global_category_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.GlobalCategoryId).HasColumnName("global_category_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.StationId).HasColumnName("station_id");

            entity.HasOne(d => d.GlobalCategory).WithMany(p => p.PdvStationCategories)
                .HasForeignKey(d => d.GlobalCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_station_categories_global_category_id_fkey");

            entity.HasOne(d => d.Station).WithMany(p => p.PdvStationCategories)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_station_categories_station_id_fkey");
        });

        modelBuilder.Entity<PdvTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_tables_pkey");

            entity.ToTable("pdv_tables", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Company).WithMany(p => p.PdvTables)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_tables_company_id_fkey");
        });

        modelBuilder.Entity<PdvWaiter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pdv_waiters_pkey");

            entity.ToTable("pdv_waiters", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");

            entity.HasOne(d => d.Company).WithMany(p => p.PdvWaiters)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pdv_waiters_company_id_fkey");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("receipts_pkey");

            entity.ToTable("receipts", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IssuedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issued_at");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Sale).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("receipts_sale_id_fkey");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_pkey");

            entity.ToTable("sales", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.SaleDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sale_date");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Company).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_company_id_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("sales_customer_id_fkey");

            entity.HasOne(d => d.Seller).WithMany(p => p.Sales)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("sales_seller_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Sales)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_status_id_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Sales)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_warehouse_id_fkey");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sale_details_pkey");

            entity.ToTable("sale_details", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchId).HasColumnName("batch_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Batch).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.BatchId)
                .HasConstraintName("sale_details_batch_id_fkey");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_details_sale_id_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_details_sku_id_fkey");
        });

        modelBuilder.Entity<SaleStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sale_statuses_pkey");

            entity.ToTable("sale_statuses", "sales");

            entity.HasIndex(e => e.Code, "sale_statuses_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sellers_pkey");

            entity.ToTable("sellers", "sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.Company).WithMany(p => p.Sellers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sellers_company_id_fkey");
        });

        modelBuilder.Entity<SkuAttributeValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sku_attribute_values_pkey");

            entity.ToTable("sku_attribute_values", "inventory");

            entity.HasIndex(e => new { e.SkuId, e.AttributeId }, "sku_attribute_values_sku_id_attribute_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .HasColumnName("value");

            entity.HasOne(d => d.Attribute).WithMany(p => p.SkuAttributeValues)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sku_attribute_values_attribute_id_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.SkuAttributeValues)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sku_attribute_values_sku_id_fkey");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stocks_pkey");

            entity.ToTable("stocks", "inventory");

            entity.HasIndex(e => new { e.WarehouseId, e.SkuId, e.BatchId }, "stocks_warehouse_id_sku_id_batch_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchId).HasColumnName("batch_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_updated");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.ReservedQuantity)
                .HasPrecision(10, 2)
                .HasColumnName("reserved_quantity");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Batch).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.BatchId)
                .HasConstraintName("stocks_batch_id_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stocks_sku_id_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stocks_warehouse_id_fkey");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("warehouses_pkey");

            entity.ToTable("warehouses", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Company).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("warehouses_company_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
