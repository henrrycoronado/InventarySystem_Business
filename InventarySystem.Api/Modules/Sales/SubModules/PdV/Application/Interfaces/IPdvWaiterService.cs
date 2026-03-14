using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;

public interface IPdvWaiterService
{
    Task<IEnumerable<PdvWaiterDto>> GetAllByCompanyAsync(int companyId);
    Task<PdvWaiterDto?> GetByIdAsync(int id, int companyId);
    Task<PdvWaiterDto> CreateAsync(PdvWaiterCreateDto dto);
    Task DeactivateAsync(int id);
}