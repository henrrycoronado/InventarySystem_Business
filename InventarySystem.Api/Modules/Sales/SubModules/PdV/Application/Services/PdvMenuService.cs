using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Services;

public class PdvMenuService(IPdvMenuRepository repository) : IPdvMenuService
{
    public async Task<IEnumerable<PdvMenuDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await repository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<PdvMenuDto?> GetByIdAsync(int id, int companyId)
    {
        var item = await repository.GetByIdAsync(id, companyId);
        return item is null ? null : Map(item);
    }

    public async Task<PdvMenuDto> CreateAsync(PdvMenuCreateDto dto)
    {
        var entity = PdvMenuEntity.Create(dto.CompanyId, dto.Name);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static PdvMenuDto Map(PdvMenuEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, Name = e.Name, CreatedAt = e.CreatedAt
    };
}