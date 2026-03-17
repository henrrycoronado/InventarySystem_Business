using InventarySystem.Api.src.Core.Application.DTOs;
using InventarySystem.Api.src.Core.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.src.Core.Presentation.Controllers;

[ApiController]
[Route("api/global-products")]
public class GlobalProductController(IGlobalProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? companyId)
    {
        var result = await service.GetAllActiveAsync(companyId);
        return Ok(ApiResponse<IEnumerable<GlobalProductDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"GlobalProduct {id} not found"));
        return Ok(ApiResponse<GlobalProductDto>.Ok(result));
    }

    [HttpGet("upc/{upc}")]
    public async Task<IActionResult> GetByUpc(string upc)
    {
        var result = await service.GetByUpcAsync(upc);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"GlobalProduct with UPC {upc} not found"));
        return Ok(ApiResponse<GlobalProductDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GlobalProductCreateDto dto)
    {
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<GlobalProductDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        try
        {
            await service.DeactivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.Fail($"GlobalProduct {id} not found"));
        }
    }
}