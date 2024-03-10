using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DiscountElectronicsApi.Models
{
    public partial class discount_electronicContext : DbContext
    {
        public discount_electronicContext()
        {
        }

        public discount_electronicContext(DbContextOptions<discount_electronicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AllCharacteristic> AllCharacteristics { get; set; } = null!;
        public virtual DbSet<AllGood> AllGoods { get; set; } = null!;
        public virtual DbSet<Branch> Branches { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Characteristic> Characteristics { get; set; } = null!;
        public virtual DbSet<CharacteristicName> CharacteristicNames { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Good> Goods { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-VIC\\SQLEXPRESS;Initial Catalog=discount_electronic;Persist Security Info=True;User ID=sa;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllCharacteristic>(entity =>
            {
                entity.HasKey(e => e.IdAllCharacteristic)
                    .HasName("PK_All_Characteristic");

                entity.ToTable("All_Characteristics");

                entity.Property(e => e.IdAllCharacteristic).HasColumnName("ID_All_Characteristic");

                entity.Property(e => e.IdCharacteristics).HasColumnName("ID_Characteristics");

                entity.Property(e => e.IdGoods).HasColumnName("ID_Goods");
            });

            modelBuilder.Entity<AllGood>(entity =>
            {
                entity.ToView("All_Goods");

                entity.Property(e => e.IdGoods).HasColumnName("ID_Goods");

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.Category).IsUnicode(false);

                entity.Property(e => e.IdCharacteristics).HasColumnName("ID_Characteristics");

                entity.Property(e => e.CharactericName)
                    .IsUnicode(false)
                    .HasColumnName("Characteric_Name");

                entity.Property(e => e.Characteristics).IsUnicode(false);

                entity.Property(e => e.Goods).IsUnicode(false);
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.IdBranches);

                entity.Property(e => e.IdBranches).HasColumnName("ID_Branches");

                entity.Property(e => e.Address).IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory);

                entity.ToTable("Category");

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.CategoryName)
                    .IsUnicode(false)
                    .HasColumnName("Category_Name");
            });

            modelBuilder.Entity<Characteristic>(entity =>
            {
                entity.HasKey(e => e.IdCharacteristics);

                entity.Property(e => e.IdCharacteristics).HasColumnName("ID_Characteristics");

                entity.Property(e => e.IdCharacteristicName).HasColumnName("ID_Characteristic_name");

                entity.Property(e => e.Values)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CharacteristicName>(entity =>
            {
                entity.HasKey(e => e.IdCharacteristicName);

                entity.ToTable("Characteristic_name");

                entity.Property(e => e.IdCharacteristicName).HasColumnName("ID_Characteristic_name");

                entity.Property(e => e.CharacteristicsName)
                    .IsUnicode(false)
                    .HasColumnName("Characteristics_name");

                entity.Property(e => e.Unit).IsUnicode(false);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry);

                entity.ToTable("Country");

                entity.Property(e => e.IdCountry).HasColumnName("ID_Country");

                entity.Property(e => e.CountryName)
                    .IsUnicode(false)
                    .HasColumnName("Country_name");
            });

            modelBuilder.Entity<Good>(entity =>
            {
                entity.HasKey(e => e.IdGoods);

                entity.Property(e => e.IdGoods).HasColumnName("ID_Goods");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.IdManufacturer).HasColumnName("ID_Manufacturer");

                entity.Property(e => e.Model).IsUnicode(false);

                entity.Property(e => e.Photo).IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(38, 2)");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(e => e.IdManufacturer);

                entity.ToTable("Manufacturer");

                entity.Property(e => e.IdManufacturer).HasColumnName("ID_Manufacturer");

                entity.Property(e => e.IdCountry).HasColumnName("ID_Country");

                entity.Property(e => e.ManufacturerName)
                    .IsUnicode(false)
                    .HasColumnName("Manufacturer_name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder);

                entity.ToTable("Order");

                entity.Property(e => e.IdOrder).HasColumnName("ID_Order");

                entity.Property(e => e.IdBranches).HasColumnName("ID_Branches");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.IdStatus).HasColumnName("ID_Status");

                entity.Property(e => e.OrderCost)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("Order_cost");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("Order_date");

                entity.Property(e => e.OrderNumber)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("Order_number");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.IdOrderStatus);

                entity.ToTable("Order_status");

                entity.Property(e => e.IdOrderStatus).HasColumnName("ID_Order_status");

                entity.Property(e => e.OrderStatusName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Order_status_name");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.IdUsers);

                entity.ToTable("Profile");

                entity.Property(e => e.IdUsers)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Users");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.NumPhone)
                    .HasMaxLength(15)
                    .HasColumnName("Num_Phone");

                entity.Property(e => e.SecondName)
                    .IsUnicode(false)
                    .HasColumnName("Second_name");

                entity.Property(e => e.Surname).IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.ToTable("Role");

                entity.HasIndex(e => e.RoleName, "UQ_Role")
                    .IsUnique();

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("Role_Name");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.IdShoppingCart);

                entity.ToTable("Shopping_cart");

                entity.Property(e => e.IdShoppingCart).HasColumnName("ID_Shopping_cart");

                entity.Property(e => e.IdGoods).HasColumnName("ID_Goods");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUsers);

                entity.Property(e => e.IdUsers).HasColumnName("ID_Users");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
