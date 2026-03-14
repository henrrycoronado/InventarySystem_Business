using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvOrderRepository
{
    Task<IEnumerable<PdvOrderEntity>> GetAllByCompanyAsync(int companyId);
    Task<PdvOrderEntity?> GetByIdAsync(int id);
    Task<PdvOrderEntity> CreateAsync(PdvOrderEntity entity);
    Task UpdateStatusAsync(int id, int statusId);
    Task CloseAsync(int id, int saleId);
    Task DeactivateAsync(int id);
}