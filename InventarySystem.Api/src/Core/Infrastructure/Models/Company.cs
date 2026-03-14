using System;
using System.Collections.Generic;

namespace InventarySystem.Api.src.Core.Infrastructure.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CompanyAttribute> CompanyAttributes { get; set; } = new List<CompanyAttribute>();

    public virtual ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();

    public virtual ICollection<PdvMenu> PdvMenus { get; set; } = new List<PdvMenu>();

    public virtual ICollection<PdvOrder> PdvOrders { get; set; } = new List<PdvOrder>();

    public virtual ICollection<PdvStation> PdvStations { get; set; } = new List<PdvStation>();

    public virtual ICollection<PdvTable> PdvTables { get; set; } = new List<PdvTable>();

    public virtual ICollection<PdvWaiter> PdvWaiters { get; set; } = new List<PdvWaiter>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Seller> Sellers { get; set; } = new List<Seller>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
