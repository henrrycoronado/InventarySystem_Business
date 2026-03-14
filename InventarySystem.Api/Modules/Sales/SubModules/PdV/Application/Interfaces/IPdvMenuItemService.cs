using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;

public interface IPdvMenuItemService
{
    Task<IEnumerable<PdvMenuItemDto>> GetAllByMenuAsync(int menuId);
    Task<PdvMenuItemDto?> GetByIdAsync(int id);
    Task<PdvMenuItemDto> CreateAsync(PdvMenuItemCreateDto dto);
    Task DeactivateAsync(int id);
}