namespace InventarySystem.Api.src.Core.Domain.Entities;

public class WarehouseEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public decimal TotalStock { get; private set; }

    internal WarehouseEntity() { }

    public static WarehouseEntity Create(int companyId, string name) =>
        new() { CompanyId = companyId, Name = name, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal WarehouseEntity Init(int id, int companyId, string name, bool isActive, DateTime createdAt)
    {
        Id = id; CompanyId = companyId; Name = name; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }

    internal WarehouseEntity WithTotalStock(decimal total)
    {
        TotalStock = total;
        return this;
    }
}