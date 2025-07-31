using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.UserId)
            .IsRequired();
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(n => n.IsRead)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
