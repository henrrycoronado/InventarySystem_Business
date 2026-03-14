namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

public class PdvOrderEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public int TableId { get; private set; }
    public int WaiterId { get; private set; }
    public int StatusId { get; private set; }
    public int? CustomerId { get; private set; }
    public int? SaleId { get; private set; }
    public DateTime OpenedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal PdvOrderEntity() { }

    public static PdvOrderEntity Create(int companyId, int tableId, int waiterId, int statusId, int? customerId) =>
        new() { CompanyId = companyId, TableId = tableId, WaiterId = waiterId, StatusId = statusId, CustomerId = customerId, OpenedAt = DateTime.Now, IsActive = true, CreatedAt = DateTime.Now };

    public void Close(int saleId) { SaleId = saleId; ClosedAt = DateTime.Now; }
    public void UpdateStatus(int statusId) => StatusId = statusId;
    public void Deactivate() => IsActive = false;

    internal PdvOrderEntity Init(int id, int companyId, int tableId, int waiterId, int statusId, int? customerId, int? saleId, DateTime openedAt, DateTime? closedAt, bool isActive, DateTime createdAt)
    {
        Id = id; CompanyId = companyId; TableId = tableId; WaiterId = waiterId;
        StatusId = statusId; CustomerId = customerId; SaleId = saleId;
        OpenedAt = openedAt; ClosedAt = closedAt; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}