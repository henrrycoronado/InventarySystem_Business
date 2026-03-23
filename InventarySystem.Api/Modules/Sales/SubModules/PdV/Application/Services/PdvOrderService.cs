using InventarySystem.Api.Modules.Sales.Domain.Entities;
using InventarySystem.Api.Modules.Sales.Domain.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Contracts;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Services;

public class PdvOrderService(
    IPdvOrderRepository orderRepository,
    IPdvOrderDetailRepository orderDetailRepository,
    IPdvMenuItemRepository menuItemRepository,
    IInventoryService inventoryService,
    ISaleRepository saleRepository,
    ISaleDetailRepository saleDetailRepository) : IPdvOrderService
{
    private const int OpenStatusId = 1;
    private const int BilledStatusId = 2;
    private const int SentStatusId = 1;
    private const int SaleConfirmedStatusId = 2;

    public async Task<IEnumerable<PdvOrderDto>> GetAllByCompanyAsync(int companyId)
    {
        var items = await orderRepository.GetAllByCompanyAsync(companyId);
        return items.Select(Map);
    }

    public async Task<PdvOrderDto?> GetByIdAsync(int id)
    {
        var item = await orderRepository.GetByIdAsync(id);
        return item is null ? null : Map(item);
    }

    public async Task<PdvOrderDto> OpenAsync(PdvOrderCreateDto dto)
    {
        var entity = PdvOrderEntity.Create(dto.CompanyId, dto.WarehouseId, dto.TableId, dto.WaiterId, OpenStatusId, dto.CustomerId);
        var created = await orderRepository.CreateAsync(entity);
        return Map(created);
    }

    public async Task<PdvOrderDetailDto> AddItemAsync(int orderId, int warehouseId, PdvOrderAddItemDto dto)
    {
        var menuItem = await menuItemRepository.GetByIdAsync(dto.MenuItemId)
            ?? throw new KeyNotFoundException($"MenuItem {dto.MenuItemId} not found");

        await inventoryService.ReserveStockAsync(menuItem.SkuId, warehouseId, dto.Quantity);

        var detail = PdvOrderDetailEntity.Create(orderId, dto.MenuItemId, dto.StationId, SentStatusId, dto.Quantity, dto.UnitPrice, dto.Notes);
        var created = await orderDetailRepository.CreateAsync(detail);
        return MapDetail(created);
    }

    public async Task<PdvOrderDetailDto> UpdateItemStatusAsync(int orderId, int detailId, int statusId)
    {
        await orderDetailRepository.UpdateStatusAsync(detailId, statusId);
        var details = await orderDetailRepository.GetAllByOrderAsync(orderId);
        var detail = details.FirstOrDefault(d => d.Id == detailId)
            ?? throw new KeyNotFoundException($"Detail {detailId} not found");
        return MapDetail(detail);
    }

    public async Task<PdvOrderDto> CheckoutAsync(int orderId, int warehouseId)
    {
        var order = await orderRepository.GetByIdAsync(orderId)
            ?? throw new KeyNotFoundException($"Order {orderId} not found");

        var details = await orderDetailRepository.GetAllByOrderAsync(orderId);
        var sale = SaleEntity.Create(order.CompanyId, warehouseId, null, order.CustomerId, SaleConfirmedStatusId, null);
        var createdSale = await saleRepository.CreateAsync(sale);

        foreach (var detail in details)
        {
            var menuItem = await menuItemRepository.GetByIdAsync(detail.MenuItemId)
                ?? throw new KeyNotFoundException($"MenuItem {detail.MenuItemId} not found");

            var saleDetail = SaleDetailEntity.Create(createdSale.Id, menuItem.SkuId, null, detail.Quantity, detail.UnitPrice);
            await saleDetailRepository.CreateAsync(saleDetail);

            await inventoryService.ConfirmReservationAsync(menuItem.SkuId, warehouseId, detail.Quantity);
        }

        await orderRepository.CloseAsync(orderId, createdSale.Id);
        await orderRepository.UpdateStatusAsync(orderId, BilledStatusId);
        return Map(order);
    }

    public async Task DeactivateAsync(int id) => await orderRepository.DeactivateAsync(id);

    private static PdvOrderDto Map(PdvOrderEntity e) => new()
    {
        Id = e.Id, CompanyId = e.CompanyId, WarehouseId = e.WarehouseId,
        TableId = e.TableId, WaiterId = e.WaiterId, StatusId = e.StatusId,
        CustomerId = e.CustomerId, SaleId = e.SaleId,
        OpenedAt = e.OpenedAt, ClosedAt = e.ClosedAt, CreatedAt = e.CreatedAt
    };

    private static PdvOrderDetailDto MapDetail(PdvOrderDetailEntity e) => new()
    {
        Id = e.Id, OrderId = e.OrderId, MenuItemId = e.MenuItemId, StationId = e.StationId,
        StatusId = e.StatusId, Quantity = e.Quantity, UnitPrice = e.UnitPrice,
        Notes = e.Notes, CreatedAt = e.CreatedAt
    };
}