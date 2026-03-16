using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId:int}/pdv/stations")]
public class PdvStationController(IPdvStationService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int companyId)
    {
        var result = await service.GetAllByCompanyAsync(companyId);
        return Ok(ApiResponse<IEnumerable<PdvStationDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int companyId, int id)
    {
        var result = await service.GetByIdAsync(id, companyId);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"Station {id} not found"));
        return Ok(ApiResponse<PdvStationDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int companyId, [FromBody] PdvStationCreateDto dto)
    {
        dto.CompanyId = companyId;
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { companyId, id = result.Id }, ApiResponse<PdvStationDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int companyId, int id)
    {
        try { await service.DeactivateAsync(id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<object>.Fail($"Station {id} not found")); }
    }
}

[ApiController]
[Route("api/pdv/stations/{stationId:int}/categories")]
public class PdvStationCategoryController(IPdvStationCategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int stationId)
    {
        var result = await service.GetAllByStationAsync(stationId);
        return Ok(ApiResponse<IEnumerable<PdvStationCategoryDto>>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int stationId, [FromBody] PdvStationCategoryCreateDto dto)
    {
        dto.StationId = stationId;
        var result = await service.CreateAsync(dto);
        return Ok(ApiResponse<PdvStationCategoryDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int stationId, int id)
    {
        try { await service.DeactivateAsync(id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<object>.Fail($"StationCategory {id} not found")); }
    }
}
