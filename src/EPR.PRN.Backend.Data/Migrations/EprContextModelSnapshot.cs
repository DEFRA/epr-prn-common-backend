﻿// <auto-generated />
using System;
using EPR.PRN.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    [DbContext(typeof(EprContext))]
    partial class EprContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.Eprn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccreditationNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("AccreditationYear")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DecemberWaste")
                        .HasColumnType("bit");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsExport")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IssuedByOrg")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IssuerNotes")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("IssuerReference")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ObligationYear")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OrganisationName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PackagingProducer")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PrnNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PrnSignatory")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PrnSignatoryPosition")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PrnStatusId")
                        .HasColumnType("int");

                    b.Property<string>("ProcessToBeUsed")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ProducerAgency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ReprocessingSite")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ReprocessorExporterAgency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Signature")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("StatusUpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("TonnageValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.HasIndex("PrnNumber")
                        .IsUnique();

                    b.ToTable("Prn");
                });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.Material", b =>
                {
                    b.Property<string>("MaterialName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MaterialCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("MaterialName");

                    b.ToTable("Material");

                    b.HasData(
                        new
                        {
                            MaterialName = "Plastic",
                            MaterialCode = "PL"
                        },
                        new
                        {
                            MaterialName = "Wood",
                            MaterialCode = "WD"
                        },
                        new
                        {
                            MaterialName = "Aluminium",
                            MaterialCode = "AL"
                        },
                        new
                        {
                            MaterialName = "Steel",
                            MaterialCode = "ST"
                        },
                        new
                        {
                            MaterialName = "Paper",
                            MaterialCode = "PC"
                        },
                        new
                        {
                            MaterialName = "Glass",
                            MaterialCode = "GL"
                        });
                });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.ObligationCalculation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CalculatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("MaterialObligationValue")
                        .HasColumnType("int");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Tonnage")
                        .HasColumnType("float");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ObligationCalculations");
                });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.PEprNpwdSync", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("PRNId")
                        .HasColumnType("int");

                    b.Property<int>("PRNStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PEprNpwdSync");
                });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.PrnStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StatusDescription")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("PrnStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StatusDescription = "Prn Accepted",
                            StatusName = "ACCEPTED"
                        },
                        new
                        {
                            Id = 2,
                            StatusDescription = "Prn Rejected",
                            StatusName = "REJECTED"
                        },
                        new
                        {
                            Id = 3,
                            StatusDescription = "Prn Cancelled",
                            StatusName = "CANCELLED"
                        },
                        new
                        {
                            Id = 4,
                            StatusDescription = "Prn Awaiting Acceptance",
                            StatusName = "AWAITINGACCEPTANCE"
                        });
                });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.PrnStatusHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid>("CreatedByOrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedByUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("PrnIdFk")
                        .HasColumnType("int");

                    b.Property<int>("PrnStatusIdFk")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PrnStatusHistory");
                });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.RecyclingTarget", b =>
                {
                    b.Property<int>("Year")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Year"));

                    b.Property<decimal>("AluminiumTarget")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("GlassRemeltTarget")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("GlassTarget")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("PaperTarget")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("PlasticTarget")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("SteelTarget")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("WoodTarget")
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("Year");

                    b.ToTable("RecyclingTargets");

                    b.HasData(
                        new
                        {
                            Year = 2025,
                            AluminiumTarget = 0.61m,
                            GlassRemeltTarget = 0.75m,
                            GlassTarget = 0.74m,
                            PaperTarget = 0.75m,
                            PlasticTarget = 0.55m,
                            SteelTarget = 0.8m,
                            WoodTarget = 0.45m
                        },
                        new
                        {
                            Year = 2026,
                            AluminiumTarget = 0.62m,
                            GlassRemeltTarget = 0.76m,
                            GlassTarget = 0.76m,
                            PaperTarget = 0.77m,
                            PlasticTarget = 0.57m,
                            SteelTarget = 0.81m,
                            WoodTarget = 0.46m
                        },
                        new
                        {
                            Year = 2027,
                            AluminiumTarget = 0.63m,
                            GlassRemeltTarget = 0.77m,
                            GlassTarget = 0.78m,
                            PaperTarget = 0.79m,
                            PlasticTarget = 0.59m,
                            SteelTarget = 0.82m,
                            WoodTarget = 0.47m
                        },
                        new
                        {
                            Year = 2028,
                            AluminiumTarget = 0.64m,
                            GlassRemeltTarget = 0.78m,
                            GlassTarget = 0.8m,
                            PaperTarget = 0.81m,
                            PlasticTarget = 0.61m,
                            SteelTarget = 0.83m,
                            WoodTarget = 0.48m
                        },
                        new
                        {
                            Year = 2029,
                            AluminiumTarget = 0.65m,
                            GlassRemeltTarget = 0.79m,
                            GlassTarget = 0.82m,
                            PaperTarget = 0.83m,
                            PlasticTarget = 0.63m,
                            SteelTarget = 0.84m,
                            WoodTarget = 0.49m
                        },
                        new
                        {
                            Year = 2030,
                            AluminiumTarget = 0.67m,
                            GlassRemeltTarget = 0.8m,
                            GlassTarget = 0.85m,
                            PaperTarget = 0.85m,
                            PlasticTarget = 0.65m,
                            SteelTarget = 0.85m,
                            WoodTarget = 0.5m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
