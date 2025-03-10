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

                b.HasIndex("PrnIdFk");

                b.ToTable("PrnStatusHistory");
            });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.RecyclingTarget", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("MaterialNameRT")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<decimal>("Target")
                    .HasColumnType("decimal(5,2)");

                b.Property<int>("Year")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("RecyclingTargets");

                b.HasData(
                    new
                    {
                        Id = 1,
                        MaterialNameRT = "Paper",
                        Target = 0.75m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 2,
                        MaterialNameRT = "Paper",
                        Target = 0.77m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 3,
                        MaterialNameRT = "Paper",
                        Target = 0.79m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 4,
                        MaterialNameRT = "Paper",
                        Target = 0.81m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 5,
                        MaterialNameRT = "Paper",
                        Target = 0.83m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 6,
                        MaterialNameRT = "Paper",
                        Target = 0.85m,
                        Year = 2030
                    },
                    new
                    {
                        Id = 7,
                        MaterialNameRT = "Glass",
                        Target = 0.74m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 8,
                        MaterialNameRT = "Glass",
                        Target = 0.76m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 9,
                        MaterialNameRT = "Glass",
                        Target = 0.78m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 10,
                        MaterialNameRT = "Glass",
                        Target = 0.8m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 11,
                        MaterialNameRT = "Glass",
                        Target = 0.82m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 12,
                        MaterialNameRT = "Glass",
                        Target = 0.85m,
                        Year = 2030
                    },
                    new
                    {
                        Id = 13,
                        MaterialNameRT = "Aluminium",
                        Target = 0.61m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 14,
                        MaterialNameRT = "Aluminium",
                        Target = 0.62m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 15,
                        MaterialNameRT = "Aluminium",
                        Target = 0.63m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 16,
                        MaterialNameRT = "Aluminium",
                        Target = 0.64m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 17,
                        MaterialNameRT = "Aluminium",
                        Target = 0.65m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 18,
                        MaterialNameRT = "Aluminium",
                        Target = 0.67m,
                        Year = 2030
                    },
                    new
                    {
                        Id = 19,
                        MaterialNameRT = "Steel",
                        Target = 0.8m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 20,
                        MaterialNameRT = "Steel",
                        Target = 0.81m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 21,
                        MaterialNameRT = "Steel",
                        Target = 0.82m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 22,
                        MaterialNameRT = "Steel",
                        Target = 0.83m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 23,
                        MaterialNameRT = "Steel",
                        Target = 0.84m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 24,
                        MaterialNameRT = "Steel",
                        Target = 0.85m,
                        Year = 2030
                    },
                    new
                    {
                        Id = 25,
                        MaterialNameRT = "Plastic",
                        Target = 0.55m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 26,
                        MaterialNameRT = "Plastic",
                        Target = 0.57m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 27,
                        MaterialNameRT = "Plastic",
                        Target = 0.59m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 28,
                        MaterialNameRT = "Plastic",
                        Target = 0.61m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 29,
                        MaterialNameRT = "Plastic",
                        Target = 0.63m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 30,
                        MaterialNameRT = "Plastic",
                        Target = 0.65m,
                        Year = 2030
                    },
                    new
                    {
                        Id = 31,
                        MaterialNameRT = "Wood",
                        Target = 0.45m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 32,
                        MaterialNameRT = "Wood",
                        Target = 0.46m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 33,
                        MaterialNameRT = "Wood",
                        Target = 0.47m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 34,
                        MaterialNameRT = "Wood",
                        Target = 0.48m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 35,
                        MaterialNameRT = "Wood",
                        Target = 0.49m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 36,
                        MaterialNameRT = "Wood",
                        Target = 0.5m,
                        Year = 2030
                    },
                    new
                    {
                        Id = 37,
                        MaterialNameRT = "GlassRemelt",
                        Target = 0.75m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 38,
                        MaterialNameRT = "GlassRemelt",
                        Target = 0.76m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 39,
                        MaterialNameRT = "GlassRemelt",
                        Target = 0.77m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 40,
                        MaterialNameRT = "GlassRemelt",
                        Target = 0.78m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 41,
                        MaterialNameRT = "GlassRemelt",
                        Target = 0.79m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 42,
                        MaterialNameRT = "GlassRemelt",
                        Target = 0.8m,
                        Year = 2030
                    },
                    new
                    {
                        Id = 43,
                        MaterialNameRT = "FibreComposite",
                        Target = 0.75m,
                        Year = 2025
                    },
                    new
                    {
                        Id = 44,
                        MaterialNameRT = "FibreComposite",
                        Target = 0.77m,
                        Year = 2026
                    },
                    new
                    {
                        Id = 45,
                        MaterialNameRT = "FibreComposite",
                        Target = 0.79m,
                        Year = 2027
                    },
                    new
                    {
                        Id = 46,
                        MaterialNameRT = "FibreComposite",
                        Target = 0.81m,
                        Year = 2028
                    },
                    new
                    {
                        Id = 47,
                        MaterialNameRT = "FibreComposite",
                        Target = 0.83m,
                        Year = 2029
                    },
                    new
                    {
                        Id = 48,
                        MaterialNameRT = "FibreComposite",
                        Target = 0.85m,
                        Year = 2030
                    });
            });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.PrnStatusHistory", b =>
            {
                b.HasOne("EPR.PRN.Backend.Data.DataModels.Eprn", null)
                    .WithMany("PrnStatusHistories")
                    .HasForeignKey("PrnIdFk")
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();
            });

            modelBuilder.Entity("EPR.PRN.Backend.Data.DataModels.Eprn", b =>
            {
                b.Navigation("PrnStatusHistories");
            });
#pragma warning restore 612, 618
        }
    }
}
