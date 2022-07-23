using System.Runtime.CompilerServices;
using Auth.Domain.Aggregates;
using Auth.Domain.Entities;
using Auth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.EF;

public class DbConfiguration : 
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<Staff>
{
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.ToTable("app_user");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.EmailConfirmed).HasColumnName("email_confirmed");
        builder.Property(x => x.CallingCode).HasColumnName("calling_code").IsRequired(false);
        builder.Property(x => x.Phone).HasColumnName("phone").IsRequired(false);
        builder.Property(x => x.PhoneConfirmed).HasColumnName("phone_confirmed");
        builder.OwnsOne(x => x.Password).Property(x => x.HashPassword).HasColumnName("hash_password");
        builder.OwnsOne(x => x.Password).Property(x => x.Salt).HasColumnName("salt");
        builder.Property(x => x.SecurityStamp).HasColumnName("securitystamp");   
        builder.Property(x => x.RefreshToken).HasColumnName("refresh_token");   
        builder.Property(x => x.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time");
        builder.Property(x => x.UpdateTime).HasColumnName("update_time");
        builder.Property(x => x.DeviceToken).HasColumnName("device_token").IsRequired(false);
        builder.Property(x => x.DeviceType).HasColumnName("device_type").IsRequired(false);
        builder.Property(x => x.UpdateTime).HasColumnName("update_time").IsRequired(false);
    }

    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.HasKey(staff => staff.Id);
        builder.ToTable("staff");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.EmailConfirmed).HasColumnName("email_confirmed");
        builder.Property(x => x.CallingCode).HasColumnName("calling_code").IsRequired(false);
        builder.Property(x => x.Phone).HasColumnName("phone").IsRequired(false);
        builder.Property(x => x.PhoneConfirmed).HasColumnName("phone_confirmed");
        builder.OwnsOne(x => x.Password).Property(x => x.HashPassword).HasColumnName("hash_password");
        builder.OwnsOne(x => x.Password).Property(x => x.Salt).HasColumnName("salt");
        builder.Property(x => x.SecurityStamp).HasColumnName("securitystamp");   
        builder.Property(x => x.RefreshToken).HasColumnName("refresh_token");   
        builder.Property(x => x.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time");
        builder.Property(x => x.UpdateTime).HasColumnName("update_time");
        builder.Property(x => x.DeviceToken).HasColumnName("device_token").IsRequired(false);
        builder.Property(x => x.DeviceType).HasColumnName("device_type").IsRequired(false);
        builder.Property(x => x.UpdateTime).HasColumnName("update_time").IsRequired(false);
    }
}