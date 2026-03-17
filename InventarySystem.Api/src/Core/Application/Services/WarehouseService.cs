using InventarySystem.Api.src.Core.Application.DTOs;
using InventarySystem.Api.src.Core.Application.Interfaces;
using InventarySystem.Api.src.Core.Domain.Entities;
using InventarySystem.Api.src.Core.Domain.Interfaces;

namespace InventarySystem.Api.src.Core.Application.Services;

public class WarehouseService(IWarehouseRepository repository) : IWarehouseService
{
    public async Task<IEnumerable<WarehouseDto>> GetAllByCompanyAsync(int companyId)
    {
        var warehouses = await repository.GetAllByCompanyAsync(companyId);
        return warehouses.Select(Map);
    }

    public async Task<WarehouseDto?> GetByIdAsync(int id, int companyId)
    {
        var warehouse = await repository.GetByIdAsync(id, companyId);
        if (warehouse is null) return null;
        return Map(warehouse);
    }

    public async Task<WarehouseDto> CreateAsync(WarehouseCreateDto dto)
    {
        var entity = WarehouseEntity.Create(dto.CompanyId, dto.Name);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static WarehouseDto Map(WarehouseEntity w) => new()
    {
        Id = w.Id, CompanyId = w.CompanyId, Name = w.Name,
        CreatedAt = w.CreatedAt, TotalStock = w.TotalStock
    };
}