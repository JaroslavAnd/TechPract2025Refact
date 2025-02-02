using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace InterfaceForDB
{
    public class TaxiServiceContext : DbContext
    {
        public TaxiServiceContext() : base("name=TaxiServiceDB") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                        .HasRequired(c => c.User)
                        .WithMany(u => u.Customers)
                        .HasForeignKey(c => c.User_id);

            modelBuilder.Entity<Ride>()
                        .HasRequired(r => r.Customer)  
                        .WithMany()                   
                        .HasForeignKey(r => r.Customer_id);

            modelBuilder.Entity<Ride>()
                        .HasRequired(r => r.Driver)
                        .WithMany()
                        .HasForeignKey(r => r.Driver_id);

            modelBuilder.Entity<Ride>()
                        .ToTable("Rides")  
                        .HasKey(r => r.Id);  
          
            modelBuilder.Entity<Ride>()
                        .HasRequired(r => r.Rate)  
                        .WithMany()  
                        .HasForeignKey(r => r.Rate_id)  
                        .WillCascadeOnDelete(false);
            modelBuilder.Entity<Vehicle>()
                        .ToTable("Vehicles") 
                        .HasKey(v => v.Id);

            modelBuilder.Entity<Vehicle>()
                        .ToTable("Vehicles")   
                        .HasKey(v => v.Id);    

            modelBuilder.Entity<Vehicle>()
                        .HasRequired(v => v.Driver)  
                        .WithMany() 
                        .HasForeignKey(v => v.Driver_id)  
                        .WillCascadeOnDelete(false);  

            modelBuilder.Entity<Vehicle>()
                        .Property(v => v.Driver_id)
                        .HasColumnName("Driver_id")
                        .IsRequired();
            modelBuilder.Entity<Payment>()
                        .HasRequired(p => p.Ride) 
                        .WithMany(r => r.Payments) 
                        .HasForeignKey(p => p.Ride_id); 

            base.OnModelCreating(modelBuilder);
        }


    }
}
