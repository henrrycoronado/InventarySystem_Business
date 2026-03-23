using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId:int}/pdv/orders")]
public class PdvOrderController(IPdvOrderService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int companyId)
    {
        var result = await service.GetAllByCompanyAsync(companyId);
        return Ok(ApiResponse<IEnumerable<PdvOrderDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int companyId, int id)
    {
        var result = await service.GetByIdAsync(id);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"Order {id} not found"));
        return Ok(ApiResponse<PdvOrderDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Open(int companyId, [FromBody] PdvOrderCreateDto dto)
    {
        dto.CompanyId = companyId;
        var result = await service.OpenAsync(dto);
        return CreatedAtAction(nameof(GetById), new { companyId, id = result.Id }, ApiResponse<PdvOrderDto>.Ok(result));
    }

    [HttpPost("{id:int}/items")]
    public async Task<IActionResult> AddItem(int companyId, int id, [FromQuery] int warehouseId, [FromBody] PdvOrderAddItemDto dto)
    {
        try
        {
            var result = await service.AddItemAsync(id, warehouseId, dto);
            return Ok(ApiResponse<PdvOrderDetailDto>.Ok(result));
        }
        catch (KeyNotFoundException ex) { return NotFound(ApiResponse<object>.Fail(ex.Message)); }
        catch (InvalidOperationException ex) { return BadRequest(ApiResponse<object>.Fail(ex.Message)); }
    }

    [HttpPatch("{id:int}/items/{detailId:int}/status")]
    public async Task<IActionResult> UpdateItemStatus(int companyId, int id, int detailId, [FromQuery] int statusId)
    {
        try
        {
            var result = await service.UpdateItemStatusAsync(id, detailId, statusId);
            return Ok(ApiResponse<PdvOrderDetailDto>.Ok(result));
        }
        catch (KeyNotFoundException ex) { return NotFound(ApiResponse<object>.Fail(ex.Message)); }
    }

    [HttpPost("{id:int}/checkout")]
    public async Task<IActionResult> Checkout(int companyId, int id, [FromQuery] int warehouseId)
    {
        try
        {
            var result = await service.CheckoutAsync(id, warehouseId);
            return Ok(ApiResponse<PdvOrderDto>.Ok(result));
        }
        catch (KeyNotFoundException ex) { return NotFound(ApiResponse<object>.Fail(ex.Message)); }
        catch (InvalidOperationException ex) { return BadRequest(ApiResponse<object>.Fail(ex.Message)); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int companyId, int id)
    {
        try { await service.DeactivateAsync(id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<object>.Fail($"Order {id} not found")); }
    }
}