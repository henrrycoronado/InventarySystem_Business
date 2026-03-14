using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class CompanyProduct
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int? GlobalProductId { get; set; }

    public string? LocalNameAlias { get; set; }

    public decimal? WholesalePrice { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CompanySku> CompanySkus { get; set; } = new List<CompanySku>();

    public virtual GlobalProduct? GlobalProduct { get; set; }
}
