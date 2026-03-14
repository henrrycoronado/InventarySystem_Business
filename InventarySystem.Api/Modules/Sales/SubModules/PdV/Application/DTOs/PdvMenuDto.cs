namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

public class PdvMenuDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
}

public class PdvMenuCreateDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
}