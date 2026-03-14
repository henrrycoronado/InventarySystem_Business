namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

public class PdvTableEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public int? Capacity { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal PdvTableEntity() { }

    public static PdvTableEntity Create(int companyId, string name, int? capacity) =>
        new() { CompanyId = companyId, Name = name, Capacity = capacity, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal PdvTableEntity Init(int id, int companyId, string name, int? capacity, bool isActive, DateTime createdAt)
    {
        Id = id; CompanyId = companyId; Name = name; Capacity = capacity; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}