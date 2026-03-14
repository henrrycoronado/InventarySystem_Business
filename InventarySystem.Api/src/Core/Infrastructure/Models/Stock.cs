using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class Stock
{
    public int Id { get; set; }

    public int WarehouseId { get; set; }

    public int SkuId { get; set; }

    public int? BatchId { get; set; }

    public decimal Quantity { get; set; }

    public decimal ReservedQuantity { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Batch? Batch { get; set; }

    public virtual CompanySku Sku { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
