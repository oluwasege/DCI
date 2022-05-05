using FSDH.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSDH.Core.DataAccess.EfCore.Mapping
{
    public class CompanyTypeMapping : IEntityTypeConfiguration<CompanyType>
    {
        public void Configure(EntityTypeBuilder<CompanyType> builder)
        {
            SeedData(builder);

        }

        private void SeedData(EntityTypeBuilder<CompanyType> builder)
        {
            var dataList = new List<CompanyType> {
                new CompanyType
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Type = "INVESTMENT"
                },
                new CompanyType
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Type = "COMPANY"
                },
                new CompanyType
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Type = "FUND"
                }
            };
            builder.HasData(dataList);
        }
    }
}
