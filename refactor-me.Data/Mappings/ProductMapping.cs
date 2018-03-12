using System.Data.Entity.ModelConfiguration;
using refactor_me.Models;

namespace refactor_me.Data.Mapping
{
    internal class ProductMapping : EntityTypeConfiguration<Product>
    {
        public ProductMapping()
        {
            HasKey(c => new {c.Id});

            Property(c => c.DeliveryPrice);
            Property(c => c.Description);
            Property(c => c.Name);
            Property(c => c.Price);

            ToTable("dbo.Product");
        }
    }
}
