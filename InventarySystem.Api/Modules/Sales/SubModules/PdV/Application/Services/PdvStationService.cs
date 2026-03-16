using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Services;

public class PdvStationService(IPdvStationRepository repository) : IPdvStationService
{
    public async Task<IEnumerable<PdvStationDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await repository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<PdvStationDto?> GetByIdAsync(int id, int companyId)
    {
        var item = await repository.GetByIdAsync(id, companyId);
        return item is null ? null : Map(item);
    }

    public async Task<PdvStationDto> CreateAsync(PdvStationCreateDto dto)
    {
        var entity = PdvStationEntity.Create(dto.CompanyId, dto.Name);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static PdvStationDto Map(PdvStationEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, Name = e.Name, CreatedAt = e.CreatedAt
    };
}

public class PdvStationCategoryService(IPdvStationCategoryRepository repository) : IPdvStationCategoryService
{
    public async Task<IEnumerable<PdvStationCategoryDto>> GetAllByStationAsync(int stationId)
    {
        var items = await repository.GetAllByStationAsync(stationId);
        return items.Select(Map);
    }

    public async Task<PdvStationCategoryDto> CreateAsync(PdvStationCategoryCreateDto dto)
    {
        var entity = PdvStationCategoryEntity.Create(dto.StationId, dto.GlobalCategoryId);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static PdvStationCategoryDto Map(PdvStationCategoryEntity e) => new()
    {
        Id = e.Id, StationId = e.StationId, GlobalCategoryId = e.GlobalCategoryId, CreatedAt = e.CreatedAt
    };
}
