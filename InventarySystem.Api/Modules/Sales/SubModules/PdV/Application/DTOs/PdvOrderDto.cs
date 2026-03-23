namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

public class PdvOrderDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int TableId { get; set; }
    public int WaiterId { get; set; }
    public int StatusId { get; set; }
    public int? CustomerId { get; set; }
    public int? SaleId { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class PdvOrderCreateDto
{
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int TableId { get; set; }
    public int WaiterId { get; set; }
    public int? CustomerId { get; set; }
}

public class PdvOrderAddItemDto
{
    public int MenuItemId { get; set; }
    public int? StationId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
}