﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCoreApp.Infrastructure.Data;

namespace NetCoreApp.Infrastructure.Data.Migrations
{
    [DbContext(typeof(NetCoreAppContext))]
    partial class NetCoreAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCoreApp.Core.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategorieId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateModification")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSaisie")
                        .HasColumnType("datetime2");

                    b.Property<string>("Libelle")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<double>("Prix")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategorieId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("NetCoreApp.Core.Entities.Categorie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateModification")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSaisie")
                        .HasColumnType("datetime2");

                    b.Property<string>("Libelle")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NetCoreApp.Core.Entities.Article", b =>
                {
                    b.HasOne("NetCoreApp.Core.Entities.Categorie", "Categorie")
                        .WithMany("Articles")
                        .HasForeignKey("CategorieId")
                        .HasConstraintName("ForeignKey_Article_Categorie")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
