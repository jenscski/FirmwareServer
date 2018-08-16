﻿// <auto-generated />
using System;
using FirmwareServer.EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FirmwareServer.EntityLayer.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20180811182833_Application.FirmwareId")]
    partial class ApplicationFirmwareId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("Description");

                    b.Property<int>("DeviceTypeId");

                    b.Property<int?>("FirmwareId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DeviceTypeId");

                    b.ToTable("Application");
                });

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApMac");

                    b.Property<int?>("ApplicationId");

                    b.Property<int?>("ChipSize");

                    b.Property<int>("ChipType");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<int?>("CurrentFirmwareId");

                    b.Property<int?>("DeviceTypeId");

                    b.Property<int?>("FirmwareId");

                    b.Property<int?>("FreeSpace");

                    b.Property<DateTimeOffset>("LastOnline");

                    b.Property<string>("Name");

                    b.Property<string>("RemoteIpAddress");

                    b.Property<string>("SdkVersion");

                    b.Property<string>("StaMac");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("CurrentFirmwareId");

                    b.HasIndex("DeviceTypeId");

                    b.HasIndex("FirmwareId");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.DeviceLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<int?>("DeviceId");

                    b.Property<int>("Level");

                    b.Property<string>("Message");

                    b.HasKey("Id");

                    b.ToTable("DeviceLog");
                });

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.DeviceType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Active");

                    b.Property<int>("ChipType");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("DeviceType");
                });

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.Firmware", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApplicationId");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<byte[]>("Data");

                    b.Property<string>("Description");

                    b.Property<string>("Filename");

                    b.Property<string>("MD5");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Firmware");
                });

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.Application", b =>
                {
                    b.HasOne("FirmwareServer.EntityLayer.Models.DeviceType", "DeviceType")
                        .WithMany()
                        .HasForeignKey("DeviceTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.Device", b =>
                {
                    b.HasOne("FirmwareServer.EntityLayer.Models.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.HasOne("FirmwareServer.EntityLayer.Models.Firmware", "CurrentFirmware")
                        .WithMany()
                        .HasForeignKey("CurrentFirmwareId");

                    b.HasOne("FirmwareServer.EntityLayer.Models.DeviceType", "DeviceType")
                        .WithMany()
                        .HasForeignKey("DeviceTypeId");

                    b.HasOne("FirmwareServer.EntityLayer.Models.Firmware", "Firmware")
                        .WithMany()
                        .HasForeignKey("FirmwareId");
                });

            modelBuilder.Entity("FirmwareServer.EntityLayer.Models.Firmware", b =>
                {
                    b.HasOne("FirmwareServer.EntityLayer.Models.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
