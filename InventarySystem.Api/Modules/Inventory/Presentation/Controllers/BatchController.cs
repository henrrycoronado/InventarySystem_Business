using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/skus/{skuId:int}/batches")]
public class BatchController(IBatchService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int skuId)
    {
        var result = await service.GetAllBySkuAsync(skuId);
        return Ok(ApiResponse<IEnumerable<BatchDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int skuId, int id)
    {
        var result = await service.GetByIdAsync(id);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"Batch {id} not found"));
        return Ok(ApiResponse<BatchDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int skuId, [FromBody] BatchCreateDto dto)
    {
        dto.SkuId = skuId;
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { skuId, id = result.Id }, ApiResponse<BatchDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int skuId, int id)
    {
        try { await service.DeactivateAsync(id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<object>.Fail($"Batch {id} not found")); }
    }
}
