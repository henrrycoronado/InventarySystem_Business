using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId:int}/pdv/menus")]
public class PdvMenuController(IPdvMenuService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int companyId)
    {
        var result = await service.GetAllByCompanyAsync(companyId);
        return Ok(ApiResponse<IEnumerable<PdvMenuDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int companyId, int id)
    {
        var result = await service.GetByIdAsync(id, companyId);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"Menu {id} not found"));
        return Ok(ApiResponse<PdvMenuDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int companyId, [FromBody] PdvMenuCreateDto dto)
    {
        dto.CompanyId = companyId;
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { companyId, id = result.Id }, ApiResponse<PdvMenuDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int companyId, int id)
    {
        try
        {
            await service.DeactivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.Fail($"Menu {id} not found"));
        }
    }
}