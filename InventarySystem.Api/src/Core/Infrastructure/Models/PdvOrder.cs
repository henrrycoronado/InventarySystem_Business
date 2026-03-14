using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class PdvOrder
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int TableId { get; set; }

    public int WaiterId { get; set; }

    public int StatusId { get; set; }

    public int? CustomerId { get; set; }

    public int? SaleId { get; set; }

    public DateTime? OpenedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<PdvOrderDetail> PdvOrderDetails { get; set; } = new List<PdvOrderDetail>();

    public virtual Sale? Sale { get; set; }

    public virtual PdvOrderStatus Status { get; set; } = null!;

    public virtual PdvTable Table { get; set; } = null!;

    public virtual PdvWaiter Waiter { get; set; } = null!;
}
