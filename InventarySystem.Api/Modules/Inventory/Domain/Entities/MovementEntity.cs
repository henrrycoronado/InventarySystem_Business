using InventarySystem.Api.Modules.Inventory.Application.DTOs;

namespace InventarySystem.Api.Modules.Inventory.Domain.Entities;

public class MovementEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public int WarehouseId { get; private set; }
    public int? TargetWarehouseId { get; private set; }
    public int StatusId { get; private set; }
    public int TypeId { get; private set; }
    public DateTime MovementDate { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }
    public IEnumerable<MovementDetailExpandedDto> Details { get; private set; } = [];

    internal MovementEntity() { }

    public static MovementEntity Create(int companyId, int warehouseId, int? targetWarehouseId, int statusId, int typeId, string? notes) =>
        new() { CompanyId = companyId, WarehouseId = warehouseId, TargetWarehouseId = targetWarehouseId, StatusId = statusId, TypeId = typeId, Notes = notes, MovementDate = DateTime.Now, IsActive = true };

    public void Deactivate() => IsActive = false;

    internal MovementEntity Init(int id, int companyId, int warehouseId, int? targetWarehouseId, int statusId, int typeId, DateTime movementDate, string? notes, bool isActive)
    {
        Id = id; CompanyId = companyId; WarehouseId = warehouseId; TargetWarehouseId = targetWarehouseId;
        StatusId = statusId; TypeId = typeId; MovementDate = movementDate; Notes = notes; IsActive = isActive;
        return this;
    }

    internal MovementEntity WithDetails(IEnumerable<MovementDetailExpandedDto> details)
    {
        Details = details;
        return this;
    }
}