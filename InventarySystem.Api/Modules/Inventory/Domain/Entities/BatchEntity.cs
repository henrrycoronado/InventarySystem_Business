namespace InventarySystem.Api.Modules.Inventory.Domain.Entities;

public class BatchEntity
{
    public int Id { get; private set; }
    public int SkuId { get; private set; }
    public string BatchNumber { get; private set; } = null!;
    public DateOnly? ManufactureDate { get; private set; }
    public DateOnly? ExpirationDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal BatchEntity() { }

    public static BatchEntity Create(int skuId, string batchNumber, DateOnly? manufactureDate, DateOnly? expirationDate) =>
        new() { SkuId = skuId, BatchNumber = batchNumber, ManufactureDate = manufactureDate, ExpirationDate = expirationDate, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal BatchEntity Init(int id, int skuId, string batchNumber, DateOnly? manufactureDate, DateOnly? expirationDate, bool isActive, DateTime createdAt)
    {
        Id = id; SkuId = skuId; BatchNumber = batchNumber;
        ManufactureDate = manufactureDate; ExpirationDate = expirationDate;
        IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}
