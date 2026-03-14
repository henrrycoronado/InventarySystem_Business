using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvOrderDetailRepository
{
    Task<IEnumerable<PdvOrderDetailEntity>> GetAllByOrderAsync(int orderId);
    Task<PdvOrderDetailEntity> CreateAsync(PdvOrderDetailEntity entity);
    Task UpdateStatusAsync(int id, int statusId);
    Task DeactivateAsync(int id);
}