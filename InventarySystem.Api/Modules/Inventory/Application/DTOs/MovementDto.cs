namespace InventarySystem.Api.Modules.Inventory.Application.DTOs;

public class MovementDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int? TargetWarehouseId { get; set; }
    public int StatusId { get; set; }
    public int TypeId { get; set; }
    public DateTime MovementDate { get; set; }
    public string? Notes { get; set; }
    public IEnumerable<MovementDetailExpandedDto> Details { get; set; } = [];
}

public class MovementDetailExpandedDto
{
    public int Id { get; set; }
    public int SkuId { get; set; }
    public int? BatchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal? UnitCost { get; set; }
    public SkuExpandedDto? Sku { get; set; }
}

public class MovementCreateDto
{
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int? TargetWarehouseId { get; set; }
    public int TypeId { get; set; }
    public string? Notes { get; set; }
    public List<MovementDetailCreateDto> Details { get; set; } = [];
}

public class MovementDetailCreateDto
{
    public int SkuId { get; set; }
    public int? BatchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal? UnitCost { get; set; }
}