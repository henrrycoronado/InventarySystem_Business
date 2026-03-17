using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class CompanyProductService(ICompanyProductRepository repository) : ICompanyProductService
{
    public async Task<IEnumerable<CompanyProductDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await repository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<CompanyProductDto?> GetByIdAsync(int id, int companyId)
    {
        var item = await repository.GetByIdAsync(id, companyId);
        return item is null ? null : Map(item);
    }

    public async Task<CompanyProductDto> CreateAsync(CompanyProductCreateDto dto)
    {
        var entity = CompanyProductEntity.Create(dto.CompanyId, dto.GlobalProductId, dto.LocalNameAlias, dto.WholesalePrice);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static CompanyProductDto Map(CompanyProductEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, GlobalProductId = e.GlobalProductId,
        LocalNameAlias = e.LocalNameAlias, WholesalePrice = e.WholesalePrice, CreatedAt = e.CreatedAt,
        GlobalProduct = e.GlobalProduct,
        Skus = e.Skus.Select(s => new CompanySkuSummaryDto { Id = s.Id, InternalSku = s.InternalSku, RetailPrice = s.RetailPrice })
    };
}