using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Services;

public class PdvTableService(IPdvTableRepository repository) : IPdvTableService
{
    public async Task<IEnumerable<PdvTableDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await repository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<PdvTableDto?> GetByIdAsync(int id, int companyId)
    {
        var item = await repository.GetByIdAsync(id, companyId);
        return item is null ? null : Map(item);
    }

    public async Task<PdvTableDto> CreateAsync(PdvTableCreateDto dto)
    {
        var entity = PdvTableEntity.Create(dto.CompanyId, dto.Name, dto.Capacity);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static PdvTableDto Map(PdvTableEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, Name = e.Name,
        Capacity = e.Capacity, CreatedAt = e.CreatedAt
    };
}