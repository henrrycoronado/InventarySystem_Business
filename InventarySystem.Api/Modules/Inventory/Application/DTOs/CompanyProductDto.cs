namespace InventarySystem.Api.Modules.Inventory.Application.DTOs;

public class CompanyProductDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int GlobalProductId { get; set; }
    public string? LocalNameAlias { get; set; }
    public decimal WholesalePrice { get; set; }
    public DateTime? CreatedAt { get; set; }
    public GlobalProductExpandedDto? GlobalProduct { get; set; }
    public IEnumerable<CompanySkuSummaryDto> Skus { get; set; } = [];
}

public class GlobalProductExpandedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Brand { get; set; }
    public string? UpcBarcode { get; set; }
    public int? CategoryId { get; set; }
    public GlobalCategoryExpandedDto? Category { get; set; }
}

public class GlobalCategoryExpandedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class CompanySkuSummaryDto
{
    public int Id { get; set; }
    public string InternalSku { get; set; } = null!;
    public decimal RetailPrice { get; set; }
}

public class CompanyProductCreateDto
{
    public int CompanyId { get; set; }
    public int GlobalProductId { get; set; }
    public string? LocalNameAlias { get; set; }
    public decimal WholesalePrice { get; set; }
}