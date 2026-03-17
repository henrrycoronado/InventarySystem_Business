using InventarySystem.Api.Modules.Inventory.Application.DTOs;

namespace InventarySystem.Api.Modules.Inventory.Domain.Entities;

public record CompanySkuSummary(int Id, string InternalSku, decimal RetailPrice);

public class CompanyProductEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public int GlobalProductId { get; private set; }
    public string? LocalNameAlias { get; private set; }
    public decimal WholesalePrice { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public GlobalProductExpandedDto? GlobalProduct { get; private set; }
    public IEnumerable<CompanySkuSummary> Skus { get; private set; } = [];

    internal CompanyProductEntity() { }

    public static CompanyProductEntity Create(int companyId, int globalProductId, string? localNameAlias, decimal wholesalePrice) =>
        new() { CompanyId = companyId, GlobalProductId = globalProductId, LocalNameAlias = localNameAlias, WholesalePrice = wholesalePrice, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal CompanyProductEntity Init(int id, int companyId, int globalProductId, string? localNameAlias, decimal wholesalePrice, bool isActive, DateTime createdAt)
    {
        Id = id; CompanyId = companyId; GlobalProductId = globalProductId;
        LocalNameAlias = localNameAlias; WholesalePrice = wholesalePrice;
        IsActive = isActive; CreatedAt = createdAt;
        return this;
    }

    internal CompanyProductEntity WithExpanded(GlobalProductExpandedDto? globalProduct, IEnumerable<CompanySkuSummary> skus)
    {
        GlobalProduct = globalProduct;
        Skus = skus;
        return this;
    }
}