using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId:int}/attributes")]
public class CompanyAttributeController(ICompanyAttributeService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int companyId)
    {
        var result = await service.GetAllByCompanyAsync(companyId);
        return Ok(ApiResponse<IEnumerable<CompanyAttributeDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int companyId, int id)
    {
        var result = await service.GetByIdAsync(id, companyId);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"Attribute {id} not found"));
        return Ok(ApiResponse<CompanyAttributeDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int companyId, [FromBody] CompanyAttributeCreateDto dto)
    {
        dto.CompanyId = companyId;
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { companyId, id = result.Id }, ApiResponse<CompanyAttributeDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int companyId, int id)
    {
        try { await service.DeactivateAsync(id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<object>.Fail($"Attribute {id} not found")); }
    }
}

[ApiController]
[Route("api/skus/{skuId:int}/attributes")]
public class SkuAttributeValueController(ISkuAttributeValueService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int skuId)
    {
        var result = await service.GetAllBySkuAsync(skuId);
        return Ok(ApiResponse<IEnumerable<SkuAttributeValueDto>>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int skuId, [FromBody] SkuAttributeValueCreateDto dto)
    {
        dto.SkuId = skuId;
        var result = await service.CreateAsync(dto);
        return Ok(ApiResponse<SkuAttributeValueDto>.Ok(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int skuId, int id)
    {
        try { await service.DeactivateAsync(id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<object>.Fail($"AttributeValue {id} not found")); }
    }
}
