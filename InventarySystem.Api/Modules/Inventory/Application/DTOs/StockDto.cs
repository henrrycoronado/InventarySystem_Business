namespace InventarySystem.Api.Modules.Inventory.Application.DTOs;

public class StockDto
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int SkuId { get; set; }
    public int? BatchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal AvailableQuantity { get; set; }
    public DateTime? LastUpdated { get; set; }
    public SkuExpandedDto? Sku { get; set; }
}

public class SkuExpandedDto
{
    public int Id { get; set; }
    public string InternalSku { get; set; } = null!;
    public decimal RetailPrice { get; set; }
    public int CompanyProductId { get; set; }
    public CompanyProductExpandedDto? CompanyProduct { get; set; }
}

public class CompanyProductExpandedDto
{
    public int Id { get; set; }
    public string? LocalNameAlias { get; set; }
    public decimal WholesalePrice { get; set; }
    public GlobalProductExpandedDto? GlobalProduct { get; set; }
}

public class StockCreateDto
{
    public int WarehouseId { get; set; }
    public int SkuId { get; set; }
    public int? BatchId { get; set; }
    public decimal Quantity { get; set; }
}