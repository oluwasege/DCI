using FSDH.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSDH.Core.DataAccess.EfCore.Mapping
{
    public class SectorMapping : IEntityTypeConfiguration<Sector>
    {
        public void Configure(EntityTypeBuilder<Sector> builder)
        {
            SeedData(builder);

        }

        private void SeedData(EntityTypeBuilder<Sector> builder)
        {
            var dataList = new List<Sector> {
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Information Technology".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "MANUFACTURING".ToUpper()
                },                
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Energy".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Health Care".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Financials".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Real Estate".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Telecommunication Services".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Consumer Discretionary".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Utilities".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Materials".ToUpper()
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    Description = "Industrials".ToUpper()
                },

            };
            builder.HasData(dataList);
        }
    }
}
