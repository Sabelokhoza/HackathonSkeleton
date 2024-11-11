﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackathonAPI.Data.SeedData
{
    public class IdentityRoleConfiguation : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole
            {
                Name = "Admininstrator",
                NormalizedName = "ADMININSTRATOR",
            },
             new IdentityRole
             {
                 Name = "Client",
                 NormalizedName = "CLIENT",
             });
        }
    }
}
