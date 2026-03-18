using InventarySystem.Api.Modules.Sales.Application.DTOs;

namespace InventarySystem.Api.Modules.Sales.Domain.Entities;

public class SaleEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public int WarehouseId { get; private set; }
    public int? SellerId { get; private set; }
    public int? CustomerId { get; private set; }
    public int StatusId { get; private set; }
    public DateTime SaleDate { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public SaleStatusSummaryDto? Status { get; private set; }
    public SalePersonSummaryDto? Customer { get; private set; }
    public SalePersonSummaryDto? Seller { get; private set; }
    public IEnumerable<SaleDetailExpandedDto> Details { get; private set; } = [];

    internal SaleEntity() { }

    public static SaleEntity Create(int companyId, int warehouseId, int? sellerId, int? customerId, int statusId, string? notes) =>
        new() { CompanyId = companyId, WarehouseId = warehouseId, SellerId = sellerId, CustomerId = customerId, StatusId = statusId, Notes = notes, SaleDate = DateTime.Now, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal SaleEntity Init(int id, int companyId, int warehouseId, int? sellerId, int? customerId, int statusId, DateTime saleDate, string? notes, bool isActive, DateTime createdAt)
    {
        Id = id; CompanyId = companyId; WarehouseId = warehouseId; SellerId = sellerId;
        CustomerId = customerId; StatusId = statusId; SaleDate = saleDate;
        Notes = notes; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }

    internal SaleEntity WithExpanded(SaleStatusSummaryDto? status, SalePersonSummaryDto? customer, SalePersonSummaryDto? seller, IEnumerable<SaleDetailExpandedDto> details)
    {
        Status = status; Customer = customer; Seller = seller; Details = details;
        return this;
    }
}