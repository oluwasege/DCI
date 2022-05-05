using FSDH.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSDH.Core.DataAccess.EfCore.Mapping
{
    public class InvestorTypeMapping : IEntityTypeConfiguration<InvestorType>
    {
        public void Configure(EntityTypeBuilder<InvestorType> builder)
        {
            //builder.ToTable(nameof(FundingRound));
            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<InvestorType> builder)
        {
            var dataList = new List<InvestorType> {
                new InvestorType
                {
                    Id = Guid.NewGuid(),
                    Description = "PEER-TO-PEER LENDERS",
                    CreationTime = DateTime.Now
                },
                new InvestorType
                {
                    Id = Guid.NewGuid(),
                    Description = "ANGEL INVESTORS",
                    CreationTime = DateTime.Now
                }
            };

            builder.HasData(dataList);
        }
    }
}
