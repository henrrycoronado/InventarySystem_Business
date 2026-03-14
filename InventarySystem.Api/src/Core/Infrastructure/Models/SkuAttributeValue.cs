using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class SkuAttributeValue
{
    public int Id { get; set; }

    public int SkuId { get; set; }

    public int AttributeId { get; set; }

    public string Value { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CompanyAttribute Attribute { get; set; } = null!;

    public virtual CompanySku Sku { get; set; } = null!;
}
