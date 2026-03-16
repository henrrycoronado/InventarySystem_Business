namespace InventarySystem.Api.Modules.Inventory.Application.DTOs;

public class BatchDto
{
    public int Id { get; set; }
    public int SkuId { get; set; }
    public string BatchNumber { get; set; } = null!;
    public DateOnly? ManufactureDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class BatchCreateDto
{
    public int SkuId { get; set; }
    public string BatchNumber { get; set; } = null!;
    public DateOnly? ManufactureDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}
