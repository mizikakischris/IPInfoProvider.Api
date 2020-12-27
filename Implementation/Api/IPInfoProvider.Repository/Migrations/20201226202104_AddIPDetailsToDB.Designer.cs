﻿// <auto-generated />
using IPInfoProvider.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IPInfoProvider.Repository.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201226202104_AddIPDetailsToDB")]
    partial class AddIPDetailsToDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IPInfoProvider.Types.Models.IPDetails", b =>
                {
                    b.Property<string>("IP")
                        .HasColumnName("IP_Adress")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Continent")
                        .HasColumnName("Continent_Name")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .HasColumnName("Country_Name")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IP");

                    b.ToTable("IPDetails");
                });
#pragma warning restore 612, 618
        }
    }
}