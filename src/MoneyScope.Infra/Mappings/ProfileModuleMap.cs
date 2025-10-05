﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyScope.Domain.AccessProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Infra.Mappings
{
    public class ProfileModuleMap : IEntityTypeConfiguration<ProfileModule>
    {
        public void Configure(EntityTypeBuilder<ProfileModule> builder)
        {
            builder.ToTable("ProfileModule");

            // Chave composta
            builder.HasKey(u => new { u.ModuleId, u.ProfileId });


            // Relacionamento Profile <-> ProfileModule
            builder.HasOne(u => u.Profile)
                .WithMany(u => u.ProfileModules)
                .HasForeignKey(u => u.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Module <-> ProfileModule
            builder.HasOne(u => u.Module)
                .WithMany(u => u.ProfileModules)
                .HasForeignKey(u => u.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
