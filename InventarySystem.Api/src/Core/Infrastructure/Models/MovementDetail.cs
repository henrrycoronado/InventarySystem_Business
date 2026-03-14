using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class MovementDetail
{
    public int Id { get; set; }

    public int MovementId { get; set; }

    public int SkuId { get; set; }

    public int? BatchId { get; set; }

    public decimal Quantity { get; set; }

    public decimal? UnitCost { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Batch? Batch { get; set; }

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual Movement Movement { get; set; } = null!;

    public virtual CompanySku Sku { get; set; } = null!;
}
