namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

public class PdvStationCategoryEntity
{
    public int Id { get; private set; }
    public int StationId { get; private set; }
    public int GlobalCategoryId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal PdvStationCategoryEntity() { }

    public static PdvStationCategoryEntity Create(int stationId, int globalCategoryId) =>
        new() { StationId = stationId, GlobalCategoryId = globalCategoryId, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal PdvStationCategoryEntity Init(int id, int stationId, int globalCategoryId, bool isActive, DateTime createdAt)
    {
        Id = id; StationId = stationId; GlobalCategoryId = globalCategoryId; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}
