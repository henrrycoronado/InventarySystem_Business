using InventarySystem.Api.Modules.Inventory.Application.DTOs;

namespace InventarySystem.Api.Modules.Inventory.Application.Interfaces;

public interface IBatchService
{
    Task<IEnumerable<BatchDto>> GetAllBySkuAsync(int skuId);
    Task<BatchDto?> GetByIdAsync(int id);
    Task<BatchDto> CreateAsync(BatchCreateDto dto);
    Task DeactivateAsync(int id);
}
