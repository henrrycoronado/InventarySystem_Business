namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

public class PdvWaiterDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
}

public class PdvWaiterCreateDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
}