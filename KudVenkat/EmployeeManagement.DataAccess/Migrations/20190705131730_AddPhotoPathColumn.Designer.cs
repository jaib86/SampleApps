﻿// <auto-generated />
using EmployeeManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmployeeManagement.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190705131730_AddPhotoPathColumn")]
    partial class AddPhotoPathColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EmployeeManagement.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Department");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhotoPath");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = 1,
                            Email = "mary@techjp.com",
                            Name = "Mary"
                        },
                        new
                        {
                            Id = 2,
                            Department = 0,
                            Email = "john@techjp.com",
                            Name = "John"
                        },
                        new
                        {
                            Id = 3,
                            Department = 2,
                            Email = "sam@techjp.com",
                            Name = "Sam"
                        },
                        new
                        {
                            Id = 4,
                            Department = 1,
                            Email = "jack@techjp.com",
                            Name = "Jack"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
