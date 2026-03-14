namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

public class PdvMenuItemEntity
{
    public int Id { get; private set; }
    public int MenuId { get; private set; }
    public int SkuId { get; private set; }
    public int? StationId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal PdvMenuItemEntity() { }

    public static PdvMenuItemEntity Create(int menuId, int skuId, int? stationId) =>
        new() { MenuId = menuId, SkuId = skuId, StationId = stationId, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal PdvMenuItemEntity Init(int id, int menuId, int skuId, int? stationId, bool isActive, DateTime createdAt)
    {
        Id = id; MenuId = menuId; SkuId = skuId; StationId = stationId; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}