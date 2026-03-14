using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId:int}/pdv/waiters")]
public class PdvWaiterController(IPdvWaiterService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int companyId)
    {
        var result = await service.GetAllByCompanyAsync(companyId);
        return Ok(ApiResponse<IEnumerable<PdvWaiterDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int companyId, int id)
    {
        var result = await service.GetByIdAsync(id, companyId);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"Waiter {id} not found"));
        return Ok(ApiResponse<PdvWaiterDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int companyId, [FromBody] PdvWaiterCreateDto dto)
    {
        dto.CompanyId = companyId;
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { companyId, id = result.Id }, ApiResponse<PdvWaiterDto>.Ok(result));
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
            return NotFound(ApiResponse<object>.Fail($"Waiter {id} not found"));
        }
    }
}