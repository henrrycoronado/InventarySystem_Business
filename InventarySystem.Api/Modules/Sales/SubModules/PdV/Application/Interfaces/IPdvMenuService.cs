using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;

public interface IPdvMenuService
{
    Task<IEnumerable<PdvMenuDto>> GetAllByCompanyAsync(int companyId);
    Task<PdvMenuDto?> GetByIdAsync(int id, int companyId);
    Task<PdvMenuDto> CreateAsync(PdvMenuCreateDto dto);
    Task DeactivateAsync(int id);
}