using FSDH.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSDH.Core.DataAccess.EfCore.Mapping
{
    public class FundingRoundMapping : IEntityTypeConfiguration<FundingType>
    {
        public void Configure(EntityTypeBuilder<FundingType> builder)
        {
            //builder.ToTable(nameof(FundingRound));
            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<FundingType> builder)
        {
            var dataList = new List<FundingType> {
                new FundingType
                {
                    Id = Guid.NewGuid(),
                    Name = "PRE SEED",
                    CreationTime = DateTime.Now                    
                },
                new FundingType
                {
                    Id = Guid.NewGuid(),
                    Name = "SERIES A",
                    CreationTime = DateTime.Now
                },
                new FundingType
                {
                    Id = Guid.NewGuid(),
                    Name = "SERIES B",
                    CreationTime = DateTime.Now
                },
                new FundingType
                {
                    Id = Guid.NewGuid(),
                    Name = "SERIES C",
                    CreationTime = DateTime.Now
                },
            };

            builder.HasData(dataList);
        }
    }
}
