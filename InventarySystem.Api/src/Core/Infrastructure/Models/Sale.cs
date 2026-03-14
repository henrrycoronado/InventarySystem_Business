using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int WarehouseId { get; set; }

    public int? SellerId { get; set; }

    public int? CustomerId { get; set; }

    public int StatusId { get; set; }

    public DateTime? SaleDate { get; set; }

    public string? Notes { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<PdvOrder> PdvOrders { get; set; } = new List<PdvOrder>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual Seller? Seller { get; set; }

    public virtual SaleStatus Status { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
