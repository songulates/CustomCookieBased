using CustomCookieCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCookieCore.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasData(new AppUser
            {
                Id=1,
                UserName="Songül",
                Password="123456"
            });
            builder.Property(x => x.Password).HasMaxLength(100).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(100).IsRequired();
           
        }
        public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
        {
            public void Configure(EntityTypeBuilder<AppRole> builder)
            {
                builder.HasData(new AppRole
                {
                    Id = 1,
                    Definition="Admin"
                });
                builder.Property(x => x.Definition).HasMaxLength(200).IsRequired();
               
            }
        }
        public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
        {
            public void Configure(EntityTypeBuilder<AppUserRole> builder)
            {
                builder.HasData(new AppUserRole
                {
                    UserId=1,
                    RoleId=1
                });
                builder.HasKey(x => new { x.UserId,x.RoleId});
                builder.HasOne(x => x.AppRole).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
                builder.HasOne(x => x.AppUser).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            }
        }
    }
}
