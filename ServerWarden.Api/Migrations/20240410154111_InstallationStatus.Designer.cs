﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerWarden.Api;

#nullable disable

namespace ServerWarden.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240410154111_InstallationStatus")]
    partial class InstallationStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("ServerWarden.Api.Models.Database.ServerPermission", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ServerProfileId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "ServerProfileId");

                    b.HasIndex("ServerProfileId");

                    b.ToTable("ServerPermissions");
                });

            modelBuilder.Entity("ServerWarden.Api.Models.Database.ServerProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasBeenInstalled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InstallationPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ServerType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("ServerWarden.Api.Models.Database.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ServerWarden.Api.Models.Database.ServerPermission", b =>
                {
                    b.HasOne("ServerWarden.Api.Models.Database.ServerProfile", null)
                        .WithMany("UserPermissions")
                        .HasForeignKey("ServerProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServerWarden.Api.Models.Database.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerWarden.Api.Models.Database.ServerProfile", b =>
                {
                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("ServerWarden.Api.Models.Database.User", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
