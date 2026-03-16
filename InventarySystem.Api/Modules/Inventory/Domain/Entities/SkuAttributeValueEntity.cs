namespace InventarySystem.Api.Modules.Inventory.Domain.Entities;

public class SkuAttributeValueEntity
{
    public int Id { get; private set; }
    public int SkuId { get; private set; }
    public int AttributeId { get; private set; }
    public string Value { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal SkuAttributeValueEntity() { }

    public static SkuAttributeValueEntity Create(int skuId, int attributeId, string value) =>
        new() { SkuId = skuId, AttributeId = attributeId, Value = value, IsActive = true, CreatedAt = DateTime.Now };

    public void Deactivate() => IsActive = false;

    internal SkuAttributeValueEntity Init(int id, int skuId, int attributeId, string value, bool isActive, DateTime createdAt)
    {
        Id = id; SkuId = skuId; AttributeId = attributeId; Value = value; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}
