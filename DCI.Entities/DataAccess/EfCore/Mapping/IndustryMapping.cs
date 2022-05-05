using FSDH.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSDH.Core.DataAccess.EfCore.Mapping
{
    public class IndustryMapping : IEntityTypeConfiguration<Industry>
    {
        public void Configure(EntityTypeBuilder<Industry> builder)
        {
            SeedData(builder);

        }

        private void SeedData(EntityTypeBuilder<Industry> builder)
        {
            var dataList = new List<Industry> {
                new Industry
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Name = "INFORMATION TECHNOLOGY"
                }
                
            };
            builder.HasData(dataList);
        }
    }
}
