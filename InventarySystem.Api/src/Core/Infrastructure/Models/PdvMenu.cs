using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class PdvMenu
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<PdvMenuItem> PdvMenuItems { get; set; } = new List<PdvMenuItem>();
}
