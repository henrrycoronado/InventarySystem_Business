using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Services;

public class PdvWaiterService(IPdvWaiterRepository repository) : IPdvWaiterService
{
    public async Task<IEnumerable<PdvWaiterDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await repository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<PdvWaiterDto?> GetByIdAsync(int id, int companyId)
    {
        var item = await repository.GetByIdAsync(id, companyId);
        return item is null ? null : Map(item);
    }

    public async Task<PdvWaiterDto> CreateAsync(PdvWaiterCreateDto dto)
    {
        var entity = PdvWaiterEntity.Create(dto.CompanyId, dto.Name);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static PdvWaiterDto Map(PdvWaiterEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, Name = e.Name, CreatedAt = e.CreatedAt
    };
}