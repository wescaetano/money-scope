using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyScope.Domain.AccessProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Infra.Mappings
{
    public class ProfileUserMap : IEntityTypeConfiguration<ProfileUser>
    {
        public void Configure(EntityTypeBuilder<ProfileUser> builder)
        {
            builder.ToTable("ProfilesUsers");

            // Chave composta
            builder.HasKey(u => new { u.UserId, u.ProfileId });


            // Relacionamento Profile <-> ProfileModule
            builder.HasOne(u => u.Profile)
                .WithMany(u => u.ProfilesUsers)
                .HasForeignKey(u => u.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Module <-> ProfileModule
            builder.HasOne(u => u.User)
                .WithMany(u => u.ProfilesUsers)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
