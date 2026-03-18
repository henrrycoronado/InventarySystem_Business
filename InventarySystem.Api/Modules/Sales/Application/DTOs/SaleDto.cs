namespace InventarySystem.Api.Modules.Sales.Application.DTOs;

public class SaleDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int? SellerId { get; set; }
    public int? CustomerId { get; set; }
    public int StatusId { get; set; }
    public DateTime SaleDate { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
    public SaleStatusSummaryDto? Status { get; set; }
    public SalePersonSummaryDto? Customer { get; set; }
    public SalePersonSummaryDto? Seller { get; set; }
    public IEnumerable<SaleDetailExpandedDto> Details { get; set; } = [];
}

public class SaleStatusSummaryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class SalePersonSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
}

public class SaleDetailExpandedDto
{
    public int Id { get; set; }
    public int SkuId { get; set; }
    public int? BatchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public SaleSkuExpandedDto? Sku { get; set; }
}

public class SaleSkuExpandedDto
{
    public int Id { get; set; }
    public string InternalSku { get; set; } = null!;
    public decimal RetailPrice { get; set; }
    public string? ProductName { get; set; }
    public string? LocalNameAlias { get; set; }
}

public class SaleCreateDto
{
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int? SellerId { get; set; }
    public int? CustomerId { get; set; }
    public string? Notes { get; set; }
    public List<SaleDetailCreateDto> Details { get; set; } = [];
}

public class SaleDetailCreateDto
{
    public int SkuId { get; set; }
    public int? BatchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}