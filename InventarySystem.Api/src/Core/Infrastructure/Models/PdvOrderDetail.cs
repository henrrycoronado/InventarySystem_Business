using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class PdvOrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int MenuItemId { get; set; }

    public int? StationId { get; set; }

    public int StatusId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string? Notes { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual PdvMenuItem MenuItem { get; set; } = null!;

    public virtual PdvOrder Order { get; set; } = null!;

    public virtual PdvStation? Station { get; set; }

    public virtual PdvItemStatus Status { get; set; } = null!;
}
