using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class CompanySku
{
    public int Id { get; set; }

    public int CompanyProductId { get; set; }

    public string InternalSku { get; set; } = null!;

    public decimal? RetailPrice { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();

    public virtual CompanyProduct CompanyProduct { get; set; } = null!;

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual ICollection<MovementDetail> MovementDetails { get; set; } = new List<MovementDetail>();

    public virtual ICollection<PdvMenuItem> PdvMenuItems { get; set; } = new List<PdvMenuItem>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual ICollection<SkuAttributeValue> SkuAttributeValues { get; set; } = new List<SkuAttributeValue>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
