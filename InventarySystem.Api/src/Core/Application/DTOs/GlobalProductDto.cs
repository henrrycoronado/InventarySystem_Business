namespace InventarySystem.Api.src.Core.Application.DTOs;

public class GlobalProductDto
{
    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Brand { get; set; }
    public string? UpcBarcode { get; set; }
    public DateTime? CreatedAt { get; set; }
    public GlobalCategoryExpandedDto? Category { get; set; }
    public bool ReferencedByCompany { get; set; }
}

public class GlobalCategoryExpandedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class GlobalProductCreateDto
{
    public int? CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Brand { get; set; }
    public string? UpcBarcode { get; set; }
}