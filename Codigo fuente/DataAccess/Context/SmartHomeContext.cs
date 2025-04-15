using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess.Context
{
    public class SmartHomeContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Home> Homes { get; set; } 
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<HomeOwnerPermission> HomeOwnerPermissions { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<HomeDevice> HomeDevices { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Room> Room { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>(u => u.Role)       
                .HasValue<Admin>("admin") 
                .HasValue<HomeOwner>("homeOwner")        
                .HasValue<CompanyOwner>("companyOwner");

            modelBuilder.Entity<Device>()
                .HasDiscriminator<string>(d => d.DeviceType)
                .HasValue<Camera>("Camera")
                .HasValue<WindowSensor>("Window Sensor")
                .HasValue<SmartLamp>("Smart Lamp")
                .HasValue<MotionSensor>("Motion Sensor");

            modelBuilder.Entity<Home>()
            .HasOne(h => h.Owner)
            .WithMany() 
            .HasForeignKey(h => h.HomeOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Home>()
            .HasMany(h => h.Members)
            .WithMany(u => u.Homes)
            .UsingEntity<Dictionary<string, object>>(
                "HomeUser",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Home>()
                    .WithMany()
                    .HasForeignKey("HomeId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

            modelBuilder.Entity<CompanyOwner>()
            .HasOne(owner => owner.Company)     
            .WithOne(company => company.Owner) 
            .HasForeignKey<CompanyOwner>(owner => owner.CompanyId);

            modelBuilder.Entity<Home>()
                .HasMany(h => h.MemberPermissions)
                .WithOne(hp => hp.Home)
                .HasForeignKey(hp => hp.HomeId);

            modelBuilder.Entity<HomeOwnerPermission>()
                .HasOne(hp => hp.HomeOwner)
                .WithMany() 
                .HasForeignKey(hp => hp.HomeOwnerId);

            modelBuilder.Entity<Device>()
            .HasOne(d => d.Company)              
            .WithMany()                        
            .HasForeignKey(d => d.CompanyId);

            modelBuilder.Entity<HomeDevice>()
            .HasKey(hd => hd.HardwareId);

            modelBuilder.Entity<HomeDevice>()
                        .HasOne(hd => hd.Device)
                        .WithMany()
                        .HasForeignKey(hd => hd.DeviceId);

            modelBuilder.Entity<Home>()
                .HasMany(h => h.HomeDevices)
                .WithOne()  
                .HasForeignKey(hd => hd.HomeId);

            modelBuilder.Entity<Home>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Home)
                .HasForeignKey(r => r.HomeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Devices)
                .WithOne(d => d.Room)
                .HasForeignKey(d => d.RoomId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@smarthome.com",
                    Password = "Password123!", 
                    Role = "admin"
                }
            );

            base.OnModelCreating(modelBuilder);
        }

        public SmartHomeContext() { }

        public SmartHomeContext(DbContextOptions options) : base(options) { }
    }
}