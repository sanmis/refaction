﻿using System.Data.Entity.ModelConfiguration;
using refactor_me.Models;

namespace refactor_me.Data.Mapping
{
    internal class ProductOptionMapping : EntityTypeConfiguration<ProductOption>
    {
        public ProductOptionMapping()
        {
            HasKey(c => new {c.Id});

            Property(c => c.Description);
            Property(c => c.Name);
            Property(c => c.ProductId);

            ToTable("dbo.ProductOption");
        }
    }
}
