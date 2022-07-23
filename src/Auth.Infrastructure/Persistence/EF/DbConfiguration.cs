using System.Runtime.CompilerServices;
using Auth.Domain.Aggregates;
using Auth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.EF;

public class DbConfiguration : 
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<Company>,
    IEntityTypeConfiguration<Department>,
    IEntityTypeConfiguration<Schedule>,
    IEntityTypeConfiguration<Shift>,
    IEntityTypeConfiguration<PunchGroup>
{
    
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.ToTable("app_user");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.EmailConfirmed).HasColumnName("email_confirmed");
        builder.Property(x => x.PasswordHash).HasColumnName("hash_password");        
        builder.Property(x => x.Salt).HasColumnName("salt");   
        builder.Property(x => x.SecurityStamp).HasColumnName("securitystamp");   
        builder.Property(x => x.RefreshToken).HasColumnName("refresh_token");   
        builder.Property(x => x.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time");
        builder.Property(x => x.Permission).HasColumnName("permission");
        builder.Property(x => x.BoardingTime).HasColumnName("boarding_time");
        builder.Property(x => x.Title).HasColumnName("title");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.HasOne(x => x.Manager).WithMany(x => x.Subordinates).HasForeignKey("approve_user_id").IsRequired(false);
        builder.HasOne(x => x.Company).WithMany(x => x.Users).HasForeignKey("company_id");
        builder.Property(x => x.UpdateTime).HasColumnName("update_time");
        builder.HasOne(x => x.Department).WithMany(x => x.Users).HasForeignKey("department_id");
        builder.HasOne(x => x.Schedule).WithMany(x => x.Users).HasForeignKey("schedule_id");
        builder.HasOne(x => x.Shift).WithMany(x => x.Users).HasForeignKey("shift_id");
        builder.HasOne(x => x.PunchGroup).WithMany(x => x.Users).HasForeignKey("punch_setting_id");
        builder.Property(x => x.DeviceToken).HasColumnName("device_token");
        builder.Property(x => x.DeviceType).HasColumnName("device_type");
    }
    
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("company");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Latitude).HasColumnName("latitude");
        builder.Property(x => x.Longitude).HasColumnName("longitude");
        builder.Property(x => x.Address).HasColumnName("formatted_address");
    }

    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("department");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.HasOne(x => x.Company).WithMany(x => x.Departments).HasForeignKey("company_id");
    }

    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("schedule");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Weekend).HasColumnName("week_end");
        builder.HasOne(x => x.Company).WithMany(x => x.Schedules).HasForeignKey("company_id");
    }

    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("shift");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.StartTime).HasColumnName("start_time");
        builder.Property(x => x.EndTime).HasColumnName("end_time");
        builder.Property(x => x.StartBreakTime).HasColumnName("start_break_time");
        builder.Property(x => x.EndBreakTime).HasColumnName("end_break_time");
        builder.Property(x => x.FlexibleRange).HasColumnName("flexible_range");
        builder.HasOne(x => x.Company).WithMany(x => x.Shifts).HasForeignKey("company_id");
    }

    public void Configure(EntityTypeBuilder<PunchGroup> builder)
    {
        builder.ToTable("punch_group");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Latitude).HasColumnName("latitude");
        builder.Property(x => x.Longitude).HasColumnName("longitude");
        builder.Property(x => x.Address).HasColumnName("formatted_address");
        builder.Property(x => x.ValidDistance).HasColumnName("valid_distance");
        builder.HasOne(x => x.Company).WithMany(x => x.PunchSettings).HasForeignKey("company_id");
    }
}