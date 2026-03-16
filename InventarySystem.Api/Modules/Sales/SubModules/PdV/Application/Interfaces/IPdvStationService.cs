using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;

public interface IPdvStationService
{
    Task<IEnumerable<PdvStationDto>> GetAllByCompanyAsync(int companyId);
    Task<PdvStationDto?> GetByIdAsync(int id, int companyId);
    Task<PdvStationDto> CreateAsync(PdvStationCreateDto dto);
    Task DeactivateAsync(int id);
}

public interface IPdvStationCategoryService
{
    Task<IEnumerable<PdvStationCategoryDto>> GetAllByStationAsync(int stationId);
    Task<PdvStationCategoryDto> CreateAsync(PdvStationCategoryCreateDto dto);
    Task DeactivateAsync(int id);
}
