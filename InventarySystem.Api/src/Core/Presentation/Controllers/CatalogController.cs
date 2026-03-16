using InventarySystem.Api.src.Core.Contracts;
using InventarySystem.Api.src.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.src.Core.Presentation.Controllers;

[ApiController]
[Route("api/catalogs")]
public class CatalogController(AppDbContext db) : ControllerBase
{
    [HttpGet("movement-statuses")]
    public async Task<IActionResult> GetMovementStatuses()
    {
        var result = await db.MovementStatuses
            .Where(s => s.IsActive == true)
            .Select(s => new { s.Id, s.Code, s.Name })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("movement-types")]
    public async Task<IActionResult> GetMovementTypes()
    {
        var result = await db.MovementTypes
            .Where(t => t.IsActive == true)
            .Select(t => new { t.Id, t.Code, t.Name, t.Operation })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("sale-statuses")]
    public async Task<IActionResult> GetSaleStatuses()
    {
        var result = await db.SaleStatuses
            .Where(s => s.IsActive == true)
            .Select(s => new { s.Id, s.Code, s.Name })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("pdv-order-statuses")]
    public async Task<IActionResult> GetPdvOrderStatuses()
    {
        var result = await db.PdvOrderStatuses
            .Where(s => s.IsActive == true)
            .Select(s => new { s.Id, s.Code, s.Name })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("pdv-item-statuses")]
    public async Task<IActionResult> GetPdvItemStatuses()
    {
        var result = await db.PdvItemStatuses
            .Where(s => s.IsActive == true)
            .Select(s => new { s.Id, s.Code, s.Name })
            .ToListAsync();
        return Ok(ApiResponse<object>.Ok(result));
    }
}
