namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;

public class PdvStationDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
}

public class PdvStationCreateDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
}

public class PdvStationCategoryDto
{
    public int Id { get; set; }
    public int StationId { get; set; }
    public int GlobalCategoryId { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class PdvStationCategoryCreateDto
{
    public int StationId { get; set; }
    public int GlobalCategoryId { get; set; }
}
