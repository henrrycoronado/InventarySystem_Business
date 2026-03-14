using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class PdvMenuItem
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public int SkuId { get; set; }

    public int? StationId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual PdvMenu Menu { get; set; } = null!;

    public virtual ICollection<PdvOrderDetail> PdvOrderDetails { get; set; } = new List<PdvOrderDetail>();

    public virtual CompanySku Sku { get; set; } = null!;

    public virtual PdvStation? Station { get; set; }
}
