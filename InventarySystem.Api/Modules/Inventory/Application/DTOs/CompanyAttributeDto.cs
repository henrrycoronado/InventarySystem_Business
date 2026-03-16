namespace InventarySystem.Api.Modules.Inventory.Application.DTOs;

public class CompanyAttributeDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
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
}

public class SkuAttributeValueCreateDto
{
    public int SkuId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;
}
