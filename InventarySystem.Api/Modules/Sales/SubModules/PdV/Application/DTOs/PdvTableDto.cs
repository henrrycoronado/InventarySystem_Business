namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

public class PdvTableDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public int? Capacity { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class PdvTableCreateDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public int? Capacity { get; set; }
}