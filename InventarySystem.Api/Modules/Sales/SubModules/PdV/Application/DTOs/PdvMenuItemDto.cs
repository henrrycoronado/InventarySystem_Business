namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

public class PdvMenuItemDto
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public int SkuId { get; set; }
    public int? StationId { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class PdvMenuItemCreateDto
{
    public int MenuId { get; set; }
    public int SkuId { get; set; }
    public int? StationId { get; set; }
}