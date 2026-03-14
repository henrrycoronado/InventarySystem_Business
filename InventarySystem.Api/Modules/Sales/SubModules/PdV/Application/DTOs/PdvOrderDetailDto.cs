namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

public class PdvOrderDetailDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public int? StationId { get; set; }
    public int StatusId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
}