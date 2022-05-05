using FSDH.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSDH.Core.DataAccess.EfCore.Mapping
{
    public class RejectionReasonMapping : IEntityTypeConfiguration<RejectionReason>
    {
        public void Configure(EntityTypeBuilder<RejectionReason> builder)
        {
            //builder.ToTable(nameof(FundingRound));
            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<RejectionReason> builder)
        {
            var dataList = new List<RejectionReason> {
                new RejectionReason
                {
                    Id = Guid.NewGuid(),
                     Reason = "INFORMATION NOT VALID"                    
                },
                new RejectionReason
                {
                    Id = Guid.NewGuid(),
                     Reason = "INFORMATION NOT CONFIRMED"
                },
                new RejectionReason
                {
                    Id = Guid.NewGuid(),
                     Reason = "FRAUD"
                },
                new RejectionReason
                {
                    Id = Guid.NewGuid(),
                     Reason = "LIED TO STAKEHOLDERS"
                }
            };

            builder.HasData(dataList);
        }
    }

}
