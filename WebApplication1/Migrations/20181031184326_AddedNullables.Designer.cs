﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(AbbRelCareContext))]
    [Migration("20181031184326_AddedNullables")]
    partial class AddedNullables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApplication1.Asset", b =>
                {
                    b.Property<Guid>("AssetId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int?>("HealthScore");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("AssetId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("WebApplication1.Characteristic", b =>
                {
                    b.Property<Guid>("CharacteristicId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CalculationType");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("DataAgeMax");

                    b.Property<int>("DataAgeScale");

                    b.Property<int?>("HealthScore");

                    b.Property<int>("HealthScoreQuality");

                    b.Property<double>("Max");

                    b.Property<double>("Min");

                    b.Property<string>("Name");

                    b.Property<decimal>("RelativeBaseValue");

                    b.Property<Guid>("SubSystemId");

                    b.Property<DateTime?>("TimeStamp");

                    b.Property<string>("Unit");

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<int>("UsedInCalculation");

                    b.Property<double?>("Value");

                    b.Property<double>("Value1");

                    b.Property<double>("Value2");

                    b.Property<double>("Value3");

                    b.Property<double>("Value4");

                    b.Property<int>("Weight");

                    b.HasKey("CharacteristicId");

                    b.HasIndex("SubSystemId");

                    b.ToTable("Characteristics");
                });

            modelBuilder.Entity("WebApplication1.SubSystem", b =>
                {
                    b.Property<Guid>("SubSystemId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AssetId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int?>("HealthScore");

                    b.Property<string>("Name");

                    b.Property<int>("SubSystemWeight");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("SubSystemId");

                    b.HasIndex("AssetId");

                    b.ToTable("SubSystems");
                });

            modelBuilder.Entity("WebApplication1.Characteristic", b =>
                {
                    b.HasOne("WebApplication1.SubSystem", "SubSystem")
                        .WithMany("Characteristics")
                        .HasForeignKey("SubSystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication1.SubSystem", b =>
                {
                    b.HasOne("WebApplication1.Asset", "Asset")
                        .WithMany("SubSystems")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
