using InventarySystem.Api.Modules.Inventory.Application.DTOs;

namespace InventarySystem.Api.Modules.Inventory.Application.Interfaces;

public interface ICompanyAttributeService
{
    Task<IEnumerable<CompanyAttributeDto>> GetAllByCompanyAsync(int companyId);
    Task<CompanyAttributeDto?> GetByIdAsync(int id, int companyId);
    Task<CompanyAttributeDto> CreateAsync(CompanyAttributeCreateDto dto);
    Task DeactivateAsync(int id);
}

public interface ISkuAttributeValueService
{
    Task<IEnumerable<SkuAttributeValueDto>> GetAllBySkuAsync(int skuId);
    Task<SkuAttributeValueDto> CreateAsync(SkuAttributeValueCreateDto dto);
    Task DeactivateAsync(int id);
}
