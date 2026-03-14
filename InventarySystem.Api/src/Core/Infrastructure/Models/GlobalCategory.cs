using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class GlobalCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<GlobalProduct> GlobalProducts { get; set; } = new List<GlobalProduct>();

    public virtual ICollection<PdvStationCategory> PdvStationCategories { get; set; } = new List<PdvStationCategory>();
}
