﻿// <auto-generated />
using I.Owe.You.Api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace I.Owe.You.Api.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20171031114544_AddMeToSummary")]
    partial class AddMeToSummary
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("I.Owe.You.Api.Model.Debt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Amount");

                    b.Property<int>("CreditorId");

                    b.Property<int>("DebtorId");

                    b.Property<string>("Reason");

                    b.Property<long>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("CreditorId");

                    b.HasIndex("DebtorId");

                    b.ToTable("Debts");
                });

            modelBuilder.Entity("I.Owe.You.Api.Model.DebtsSummary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("DebtDifference");

                    b.Property<long>("LastDebtTimestamp");

                    b.Property<int>("MeId");

                    b.Property<int>("PartnerId");

                    b.HasKey("Id");

                    b.HasIndex("MeId");

                    b.HasIndex("PartnerId");

                    b.ToTable("DebtsSummaries");
                });

            modelBuilder.Entity("I.Owe.You.Api.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("PhotoUrl");

                    b.Property<string>("Sub");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("I.Owe.You.Api.Model.Debt", b =>
                {
                    b.HasOne("I.Owe.You.Api.Model.User", "Creditor")
                        .WithMany()
                        .HasForeignKey("CreditorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("I.Owe.You.Api.Model.User", "Debtor")
                        .WithMany()
                        .HasForeignKey("DebtorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("I.Owe.You.Api.Model.DebtsSummary", b =>
                {
                    b.HasOne("I.Owe.You.Api.Model.User", "Me")
                        .WithMany()
                        .HasForeignKey("MeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("I.Owe.You.Api.Model.User", "Partner")
                        .WithMany()
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}