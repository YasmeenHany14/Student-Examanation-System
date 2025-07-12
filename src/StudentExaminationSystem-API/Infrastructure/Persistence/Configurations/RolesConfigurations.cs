using Domain.Models.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RolesConfigurations : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = Roles.UserRoleId,
                Name = Roles.Student,
                NormalizedName = Roles.Student.ToUpper(),
                ConcurrencyStamp = "8bccf7f0-8125-4d99-8072-9951990d25a4"
            },
            new IdentityRole
            {
                Id = Roles.AdminRoleId,
                Name = Roles.AdminRole,
                NormalizedName = Roles.AdminRole.ToUpper(),
                ConcurrencyStamp = "6b6366a6-3a14-4962-afe9-03c50aa87b71"
            }
        );
    }
}
