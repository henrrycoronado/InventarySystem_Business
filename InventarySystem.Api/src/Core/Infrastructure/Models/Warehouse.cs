using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class Warehouse
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual ICollection<Movement> MovementTargetWarehouses { get; set; } = new List<Movement>();

    public virtual ICollection<Movement> MovementWarehouses { get; set; } = new List<Movement>();

    public virtual ICollection<PdvOrder> PdvOrders { get; set; } = new List<PdvOrder>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
