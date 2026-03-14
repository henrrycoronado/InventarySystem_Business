namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

public class PdvWaiterEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal PdvWaiterEntity() { }

    public static PdvWaiterEntity Create(int companyId, string name) =>
        new() { CompanyId = companyId, Name = name, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal PdvWaiterEntity Init(int id, int companyId, string name, bool isActive, DateTime createdAt)
    {
        Id = id; CompanyId = companyId; Name = name; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}