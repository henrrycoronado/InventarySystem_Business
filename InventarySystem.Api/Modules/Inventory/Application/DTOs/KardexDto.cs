namespace InventarySystem.Api.Modules.Inventory.Application.DTOs;

public class KardexResponseDto
{
    public int SkuId { get; set; }
    public string InternalSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public IEnumerable<KardexEntryDto> Entries { get; set; } = [];
}

public class KardexEntryDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int TypeId { get; set; }
    public string TypeName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal BalanceAfter { get; set; }
}

public class KardexDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int SkuId { get; set; }
    public int? BatchId { get; set; }
    public int TypeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime CreatedAt { get; set; }
}