namespace InventarySystem.Api.Modules.Inventory.Application.DTOs;

public class CompanyAttributeDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public int SkuCount { get; set; }
}

public class CompanyAttributeCreateDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
}

public class SkuAttributeValueDto
{
    public int Id { get; set; }
    public int SkuId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public AttributeSummaryDto? Attribute { get; set; }
}

public class AttributeSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class SkuAttributeValueCreateDto
{
    public int SkuId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;
}
