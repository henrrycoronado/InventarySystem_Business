using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.src.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventarySystem.Api.Modules.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:int}/kardex")]
public class KardexController(IKardexService service) : ControllerBase
{
    [HttpGet("sku/{skuId:int}")]
    public async Task<IActionResult> GetBySkuAndWarehouse(int warehouseId, int skuId)
    {
        var result = await service.GetBySkuAndWarehouseAsync(skuId, warehouseId);
        if (result is null) return NotFound(ApiResponse<object>.Fail($"SKU {skuId} not found"));
        return Ok(ApiResponse<KardexResponseDto>.Ok(result));
    }
}