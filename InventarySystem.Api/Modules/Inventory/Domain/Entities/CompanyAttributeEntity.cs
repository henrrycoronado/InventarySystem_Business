namespace InventarySystem.Api.Modules.Inventory.Domain.Entities;

public class CompanyAttributeEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int SkuCount { get; private set; }

    internal CompanyAttributeEntity() { }

    public static CompanyAttributeEntity Create(int companyId, string name) =>
        new() { CompanyId = companyId, Name = name, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal CompanyAttributeEntity Init(int id, int companyId, string name, bool isActive, DateTime createdAt)
    {
        Id = id; CompanyId = companyId; Name = name; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }

    internal CompanyAttributeEntity WithSkuCount(int count)
    {
        SkuCount = count;
        return this;
    }
}
