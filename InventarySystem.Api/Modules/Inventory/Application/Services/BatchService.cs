using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class BatchService(IBatchRepository repository) : IBatchService
{
    public async Task<IEnumerable<BatchDto>> GetAllBySkuAsync(int skuId)
    {
        var items = await repository.GetAllBySkuAsync(skuId);
        return items.Select(Map);
    }

    public async Task<BatchDto?> GetByIdAsync(int id)
    {
        var item = await repository.GetByIdAsync(id);
        return item is null ? null : Map(item);
    }

    public async Task<BatchDto> CreateAsync(BatchCreateDto dto)
    {
        var entity = BatchEntity.Create(dto.SkuId, dto.BatchNumber, dto.ManufactureDate, dto.ExpirationDate);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static BatchDto Map(BatchEntity e) => new()
    {
        Id = e.Id, SkuId = e.SkuId, BatchNumber = e.BatchNumber,
        ManufactureDate = e.ManufactureDate, ExpirationDate = e.ExpirationDate, CreatedAt = e.CreatedAt
    };
}
