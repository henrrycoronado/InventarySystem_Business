using InventarySystem.Api.src.Core.Application.DTOs;

namespace InventarySystem.Api.src.Core.Application.Interfaces;

public interface IGlobalProductService
{
    Task<IEnumerable<GlobalProductDto>> GetAllActiveAsync(int? companyId = null);
    Task<GlobalProductDto?> GetByIdAsync(int id);
    Task<GlobalProductDto?> GetByUpcAsync(string upc);
    Task<GlobalProductDto> CreateAsync(GlobalProductCreateDto dto);
    Task DeactivateAsync(int id);
}