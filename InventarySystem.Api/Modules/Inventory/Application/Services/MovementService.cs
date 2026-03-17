using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class MovementService(
    IMovementRepository movementRepository,
    IStockRepository stockRepository,
    IKardexRepository kardexRepository) : IMovementService
{
    public async Task<IEnumerable<MovementDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await movementRepository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<MovementDto?> GetByIdAsync(int id)
    {
        var item = await movementRepository.GetByIdAsync(id);
        return item is null ? null : Map(item);
    }

    public async Task<MovementDto> RegisterIncomingAsync(MovementCreateDto dto)
    {
        var draftStatusId = 1;
        var movement = MovementEntity.Create(dto.CompanyId, dto.WarehouseId, dto.TargetWarehouseId, draftStatusId, dto.TypeId, dto.Notes);
        var created = await movementRepository.CreateAsync(movement);

        foreach (var detail in dto.Details)
        {
            var stock = await stockRepository.GetBySkuAndWarehouseAsync(detail.SkuId, dto.WarehouseId, detail.BatchId);
            if (stock is null)
            {
                stock = StockEntity.Create(dto.WarehouseId, detail.SkuId, detail.BatchId, detail.Quantity);
                await stockRepository.CreateAsync(stock);
            }
            else
            {
                stock.ConfirmReservation(-detail.Quantity);
                await stockRepository.UpdateAsync(stock);
            }

            var kardex = KardexEntity.Create(dto.CompanyId, dto.WarehouseId, detail.SkuId, detail.BatchId, created.Id, dto.TypeId, detail.Quantity, stock.AvailableQuantity);
            await kardexRepository.CreateAsync(kardex);
        }

        return Map(created);
    }

    public async Task<MovementDto> RegisterOutgoingAsync(MovementCreateDto dto)
    {
        var draftStatusId = 1;
        var movement = MovementEntity.Create(dto.CompanyId, dto.WarehouseId, dto.TargetWarehouseId, draftStatusId, dto.TypeId, dto.Notes);
        var created = await movementRepository.CreateAsync(movement);

        foreach (var detail in dto.Details)
        {
            var stock = await stockRepository.GetBySkuAndWarehouseAsync(detail.SkuId, dto.WarehouseId, detail.BatchId)
                ?? throw new InvalidOperationException($"Stock not found for SKU {detail.SkuId} in warehouse {dto.WarehouseId}");

            stock.Reserve(detail.Quantity);
            stock.ConfirmReservation(detail.Quantity);
            await stockRepository.UpdateAsync(stock);

            var kardex = KardexEntity.Create(dto.CompanyId, dto.WarehouseId, detail.SkuId, detail.BatchId, created.Id, dto.TypeId, detail.Quantity, stock.AvailableQuantity);
            await kardexRepository.CreateAsync(kardex);
        }

        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await movementRepository.DeactivateAsync(id);

    private static MovementDto Map(MovementEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, WarehouseId = e.WarehouseId,
        TargetWarehouseId = e.TargetWarehouseId, StatusId = e.StatusId,
        TypeId = e.TypeId, MovementDate = e.MovementDate, Notes = e.Notes,
        Details = e.Details
    };
}