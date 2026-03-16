using InventarySystem.Api.Modules.Sales.Application.DTOs;
using InventarySystem.Api.Modules.Sales.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.Domain.Entities;
using InventarySystem.Api.Modules.Sales.Domain.Interfaces;
using InventarySystem.Api.src.Core.Contracts;

namespace InventarySystem.Api.Modules.Sales.Application.Services;

public class SaleService(
    ISaleRepository saleRepository,
    ISaleDetailRepository saleDetailRepository,
    IInventoryService inventoryService) : ISaleService
{
    private const int DraftStatusId = 1;
    private const int ConfirmedStatusId = 2;
    private const int CancelledStatusId = 3;

    public async Task<IEnumerable<SaleDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await saleRepository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<SaleDto?> GetByIdAsync(int id)
    {
        var item = await saleRepository.GetByIdAsync(id);
        return item is null ? null : Map(item);
    }

    public async Task<SaleDto> CreateAsync(SaleCreateDto dto)
    {
        var entity = SaleEntity.Create(dto.CompanyId, dto.WarehouseId, dto.SellerId, dto.CustomerId, DraftStatusId, dto.Notes);
        var created = await saleRepository.CreateAsync(entity);

        foreach (var detail in dto.Details)
        {
            var saleDetail = SaleDetailEntity.Create(created.Id, detail.SkuId, detail.BatchId, detail.Quantity, detail.UnitPrice);
            await saleDetailRepository.CreateAsync(saleDetail);
        }

        return Map(created);
    }

    public async Task<SaleDto> ConfirmAsync(int id)
    {
        var sale = await saleRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Sale {id} not found");

        var details = await saleDetailRepository.GetAllBySaleAsync(id);

        foreach (var detail in details)
        {
            await inventoryService.ReserveStockAsync(detail.SkuId, sale.WarehouseId, detail.Quantity, detail.BatchId);
            await inventoryService.ConfirmReservationAsync(detail.SkuId, sale.WarehouseId, detail.Quantity, detail.BatchId);
        }

        await saleRepository.UpdateStatusAsync(id, ConfirmedStatusId);
        return Map(sale);
    }

    public async Task<SaleDto> CancelAsync(int id)
    {
        var sale = await saleRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Sale {id} not found");

        await saleRepository.UpdateStatusAsync(id, CancelledStatusId);
        return Map(sale);
    }

    public async Task DeactivateAsync(int id) => await saleRepository.DeactivateAsync(id);

    private static SaleDto Map(SaleEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, WarehouseId = e.WarehouseId,
        SellerId = e.SellerId, CustomerId = e.CustomerId, StatusId = e.StatusId,
        SaleDate = e.SaleDate, Notes = e.Notes, CreatedAt = e.CreatedAt
    };
}