using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class CompanyAttributeService(ICompanyAttributeRepository repository) : ICompanyAttributeService
{
    public async Task<IEnumerable<CompanyAttributeDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await repository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<CompanyAttributeDto?> GetByIdAsync(int id, int companyId)
    {
        var item = await repository.GetByIdAsync(id, companyId);
        return item is null ? null : Map(item);
    }

    public async Task<CompanyAttributeDto> CreateAsync(CompanyAttributeCreateDto dto)
    {
        var entity = CompanyAttributeEntity.Create(dto.CompanyId, dto.Name);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static CompanyAttributeDto Map(CompanyAttributeEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, Name = e.Name, CreatedAt = e.CreatedAt
    };
}

public class SkuAttributeValueService(ISkuAttributeValueRepository repository) : ISkuAttributeValueService
{
    public async Task<IEnumerable<SkuAttributeValueDto>> GetAllBySkuAsync(int skuId)
    {
        var items = await repository.GetAllBySkuAsync(skuId);
        return items.Select(Map);
    }

    public async Task<SkuAttributeValueDto> CreateAsync(SkuAttributeValueCreateDto dto)
    {
        var entity = SkuAttributeValueEntity.Create(dto.SkuId, dto.AttributeId, dto.Value);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static SkuAttributeValueDto Map(SkuAttributeValueEntity e) => new()
    {
        Id = e.Id, SkuId = e.SkuId, AttributeId = e.AttributeId, Value = e.Value, CreatedAt = e.CreatedAt
    };
}
