﻿// <auto-generated />
using System;
using InstPlusEntityFr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    [DbContext(typeof(DbInstagramPlus))]
    [Migration("20230531132240_TagiUzytk_Uzytk2")]
    partial class TagiUzytk_Uzytk2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("InstPlusEntityFr.Komentarz", b =>
                {
                    b.Property<int>("KomentarzId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KomentarzId"));

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("Tresc")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("UzytkownikId")
                        .HasColumnType("int");

                    b.HasKey("KomentarzId");

                    b.ToTable("Komentarze");
                });

            modelBuilder.Entity("InstPlusEntityFr.Obserwowany", b =>
                {
                    b.Property<int>("ObserwatorId")
                        .HasColumnType("int");

                    b.Property<int>("ObserwowanyId")
                        .HasColumnType("int");

                    b.HasKey("ObserwatorId", "ObserwowanyId");

                    b.ToTable("Obserwowani");
                });

            modelBuilder.Entity("InstPlusEntityFr.Obserwujacy", b =>
                {
                    b.Property<int>("ObserwatorId")
                        .HasColumnType("int");

                    b.Property<int>("ObserwowanyId")
                        .HasColumnType("int");

                    b.HasKey("ObserwatorId", "ObserwowanyId");

                    b.ToTable("Obserwujacy");
                });

            modelBuilder.Entity("InstPlusEntityFr.PolubienieKomentarza", b =>
                {
                    b.Property<int>("KomentarzId")
                        .HasColumnType("int");

                    b.Property<int>("UzytkownikId")
                        .HasColumnType("int");

                    b.HasKey("KomentarzId", "UzytkownikId");

                    b.ToTable("PolubieniaKomentarzy");
                });

            modelBuilder.Entity("InstPlusEntityFr.PolubieniePosta", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UzytkownikId")
                        .HasColumnType("int");

                    b.HasKey("PostId", "UzytkownikId");

                    b.ToTable("PolubieniaPostow");
                });

            modelBuilder.Entity("InstPlusEntityFr.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<DateTime>("DataPublikacji")
                        .HasColumnType("datetime2");

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("UzytkownikId")
                        .HasColumnType("int");

                    b.Property<string>("Zdjecie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PostId");

                    b.ToTable("Posty");
                });

            modelBuilder.Entity("InstPlusEntityFr.TagPostu", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("TagiPostow");
                });

            modelBuilder.Entity("InstPlusEntityFr.Uzytkownik", b =>
                {
                    b.Property<int>("UzytkownikId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UzytkownikId"));

                    b.Property<DateTime?>("DataVipDo")
                        .HasColumnType("datetime2");

                    b.Property<string>("Haslo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("Moderator")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Zdjecie")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UzytkownikId");

                    b.ToTable("Uzytkownicy");
                });

            modelBuilder.Entity("PostTagPostu", b =>
                {
                    b.Property<int>("PostyPostId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("PostyPostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTagPostu");
                });

            modelBuilder.Entity("PostTagPostu", b =>
                {
                    b.HasOne("InstPlusEntityFr.Post", null)
                        .WithMany()
                        .HasForeignKey("PostyPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstPlusEntityFr.TagPostu", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
