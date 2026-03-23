using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;

public interface IPdvOrderService
{
    Task<IEnumerable<PdvOrderDto>> GetAllByCompanyAsync(int companyId);
    Task<PdvOrderDto?> GetByIdAsync(int id);
    Task<PdvOrderDto> OpenAsync(PdvOrderCreateDto dto);
    Task<PdvOrderDetailDto> AddItemAsync(int orderId, int warehouseId, PdvOrderAddItemDto dto);
    Task<PdvOrderDetailDto> UpdateItemStatusAsync(int orderId, int detailId, int statusId);
    Task<PdvOrderDto> CheckoutAsync(int orderId, int warehouseId);
    Task DeactivateAsync(int id);
}