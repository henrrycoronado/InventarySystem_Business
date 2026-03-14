using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class Movement
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int WarehouseId { get; set; }

    public int? TargetWarehouseId { get; set; }

    public int StatusId { get; set; }

    public int TypeId { get; set; }

    public DateTime? MovementDate { get; set; }

    public string? Notes { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<MovementDetail> MovementDetails { get; set; } = new List<MovementDetail>();

    public virtual MovementStatus Status { get; set; } = null!;

    public virtual Warehouse? TargetWarehouse { get; set; }

    public virtual MovementType Type { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
