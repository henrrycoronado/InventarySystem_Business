using InventarySystem.Api.src.Core.Application.DTOs;
using InventarySystem.Api.src.Core.Application.Interfaces;
using InventarySystem.Api.src.Core.Domain.Entities;
using InventarySystem.Api.src.Core.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.src.Core.Application.Services;

public class GlobalProductService(IGlobalProductRepository repository, AppDbContext db) : IGlobalProductService
{
    public async Task<IEnumerable<GlobalProductDto>> GetAllActiveAsync(int? companyId = null)
    {
        var items = await repository.GetAllActiveAsync();

        HashSet<int> referencedIds = [];
        if (companyId.HasValue)
        {
            referencedIds = (await db.CompanyProducts
                .Where(cp => cp.CompanyId == companyId && cp.IsActive == true && cp.GlobalProductId != null)
                .Select(cp => cp.GlobalProductId!.Value)
                .ToListAsync()).ToHashSet();
        }

        var categories = await db.GlobalCategories.Where(c => c.IsActive == true).ToListAsync();

        return items.Select(e =>
        {
            var category = categories.FirstOrDefault(c => c.Id == e.CategoryId);
            return new GlobalProductDto
            {
                Id = e.Id, CategoryId = e.CategoryId, Name = e.Name,
                Brand = e.Brand, UpcBarcode = e.UpcBarcode, CreatedAt = e.CreatedAt,
                ReferencedByCompany = referencedIds.Contains(e.Id),
                Category = category is null ? null : new GlobalCategoryExpandedDto { Id = category.Id, Name = category.Name! }
            };
        });
    }

    public async Task<GlobalProductDto?> GetByIdAsync(int id)
    {
        var item = await repository.GetByIdAsync(id);
        return item is null ? null : Map(item);
    }

    public async Task<GlobalProductDto?> GetByUpcAsync(string upc)
    {
        var item = await repository.GetByUpcAsync(upc);
        return item is null ? null : Map(item);
    }

    public async Task<GlobalProductDto> CreateAsync(GlobalProductCreateDto dto)
    {
        var entity = GlobalProductEntity.Create(dto.CategoryId, dto.Name, dto.Brand, dto.UpcBarcode);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static GlobalProductDto Map(GlobalProductEntity e) => new()
    {
        Id = e.Id, CategoryId = e.CategoryId, Name = e.Name,
        Brand = e.Brand, UpcBarcode = e.UpcBarcode, CreatedAt = e.CreatedAt
    };
}