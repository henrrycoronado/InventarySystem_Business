using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Repositories;
using InventarySystem.Api.src.Core.Contracts;
using InventarySystem.Api.src.Core.Domain.Interfaces;
using InventarySystem.Api.src.Core.Application.Interfaces;
using InventarySystem.Api.src.Core.Application.Services;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Application.Services;
using InventarySystem.Api.Modules.Inventory.Infrastructure.Repositories;
using InventarySystem.Api.Modules.Sales.Domain.Interfaces;
using InventarySystem.Api.Modules.Sales.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.Application.Services;
using InventarySystem.Api.Modules.Sales.Infrastructure.Repositories;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Services;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IGlobalCategoryRepository, GlobalCategoryRepository>();
builder.Services.AddScoped<IGlobalProductRepository, GlobalProductRepository>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IGlobalCategoryService, GlobalCategoryService>();
builder.Services.AddScoped<IGlobalProductService, GlobalProductService>();

builder.Services.AddScoped<ICompanyProductRepository, CompanyProductRepository>();
builder.Services.AddScoped<ICompanySkuRepository, CompanySkuRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IMovementDetailRepository, MovementDetailRepository>();
builder.Services.AddScoped<IKardexRepository, KardexRepository>();
builder.Services.AddScoped<ICompanyAttributeRepository, CompanyAttributeRepository>();
builder.Services.AddScoped<ISkuAttributeValueRepository, SkuAttributeValueRepository>();
builder.Services.AddScoped<IBatchRepository, BatchRepository>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleDetailRepository, SaleDetailRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();

builder.Services.AddScoped<ICompanyProductService, CompanyProductService>();
builder.Services.AddScoped<ICompanySkuService, CompanySkuService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IMovementService, MovementService>();
builder.Services.AddScoped<IKardexService, KardexService>();
builder.Services.AddScoped<ICompanyAttributeService, CompanyAttributeService>();
builder.Services.AddScoped<ISkuAttributeValueService, SkuAttributeValueService>();
builder.Services.AddScoped<IBatchService, BatchService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();

builder.Services.AddScoped<IPdvTableRepository, PdvTableRepository>();
builder.Services.AddScoped<IPdvWaiterRepository, PdvWaiterRepository>();
builder.Services.AddScoped<IPdvMenuRepository, PdvMenuRepository>();
builder.Services.AddScoped<IPdvMenuItemRepository, PdvMenuItemRepository>();
builder.Services.AddScoped<IPdvOrderRepository, PdvOrderRepository>();
builder.Services.AddScoped<IPdvOrderDetailRepository, PdvOrderDetailRepository>();
builder.Services.AddScoped<IPdvStationRepository, PdvStationRepository>();
builder.Services.AddScoped<IPdvStationCategoryRepository, PdvStationCategoryRepository>();

builder.Services.AddScoped<IPdvTableService, PdvTableService>();
builder.Services.AddScoped<IPdvWaiterService, PdvWaiterService>();
builder.Services.AddScoped<IPdvMenuService, PdvMenuService>();
builder.Services.AddScoped<IPdvMenuItemService, PdvMenuItemService>();
builder.Services.AddScoped<IPdvOrderService, PdvOrderService>();
builder.Services.AddScoped<IPdvStationService, PdvStationService>();
builder.Services.AddScoped<IPdvStationCategoryService, PdvStationCategoryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "InventarySystem API",
        Version = "v1",
        Description = "Multimodular SaaS inventory management API"
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "InventarySystem API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors();
app.MapControllers();

app.Run();