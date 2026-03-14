using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;

public interface IPdvTableService
{
    Task<IEnumerable<PdvTableDto>> GetAllByCompanyAsync(int companyId);
    Task<PdvTableDto?> GetByIdAsync(int id, int companyId);
    Task<PdvTableDto> CreateAsync(PdvTableCreateDto dto);
    Task DeactivateAsync(int id);
}