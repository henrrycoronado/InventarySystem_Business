using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Presentation.Controllers;

[ApiController]
[Route("api/pdv/menus/{menuId:int}/items")]
public class PdvMenuItemController(IPdvMenuItemService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int menuId)
    {
        var result = await service.GetAllByMenuAsync(menuId);
        return Ok(ApiResponse<IEnumerable<PdvMenuItemDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int menuId, int id)
    {
        var result = await service.GetByIdAsync(id);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"MenuItem {id} not found"));
        return Ok(ApiResponse<PdvMenuItemDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int menuId, [FromBody] PdvMenuItemCreateDto dto)
    {
        dto.MenuId = menuId;
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { menuId, id = result.Id }, ApiResponse<PdvMenuItemDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int menuId, int id)
    {
        try
        {
            await service.DeactivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.Fail($"MenuItem {id} not found"));
        }
    }
}