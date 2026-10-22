using System;
using Alumni.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Alumni.Migrations
{
    [DbContext(typeof(AlumniDbContext))]
    partial class AlumniDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Alumni.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<DateTime>("CreatedAt")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("character varying(150)");

                b.Property<int>("GraduationYear")
                    .HasColumnType("integer");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("character varying(150)");

                b.Property<string>("Role")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasDefaultValue("alumni");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("timestamp with time zone");

                b.HasKey("Id");

                b.ToTable("users", (string)null);
            });

            modelBuilder.Entity("Alumni.Models.Announcement", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Author")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)")
                    .HasDefaultValue("Alumni Office");

                b.Property<string>("Category")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasDefaultValue("General");

                b.Property<string>("Content")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<DateTime>("CreatedAt")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                b.Property<bool>("IsActive")
                    .HasColumnType("boolean");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("character varying(200)");

                b.HasKey("Id");

                b.ToTable("announcements", (string)null);
            });

            modelBuilder.Entity("Alumni.Models.JobPosting", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Company")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)");

                b.Property<DateTime>("CreatedAt")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<bool>("IsActive")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                b.Property<string>("Location")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)")
                    .HasDefaultValue("Remote");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("character varying(150)");

                b.Property<int>("UserId")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("job_postings", (string)null);
            });

            modelBuilder.Entity("Alumni.Models.Application", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<DateTime>("CreatedAt")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                b.Property<int>("JobPostingId")
                    .HasColumnType("integer");

                b.Property<string>("Status")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasDefaultValue("applied");

                b.Property<int>("UserId")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("JobPostingId");

                b.HasIndex("UserId", "JobPostingId")
                    .IsUnique();

                b.ToTable("applications", (string)null);
            });

            modelBuilder.Entity("Alumni.Models.JobPosting", b =>
            {
                b.HasOne("Alumni.Models.User", "User")
                    .WithMany("Jobs")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("User");
            });

            modelBuilder.Entity("Alumni.Models.Application", b =>
            {
                b.HasOne("Alumni.Models.JobPosting", "JobPosting")
                    .WithMany("Applications")
                    .HasForeignKey("JobPostingId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Alumni.Models.User", "User")
                    .WithMany("Applications")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("JobPosting");
                b.Navigation("User");
            });

            modelBuilder.Entity("Alumni.Models.JobPosting", b =>
            {
                b.Navigation("Applications");
            });

            modelBuilder.Entity("Alumni.Models.User", b =>
            {
                b.Navigation("Applications");
                b.Navigation("Jobs");
            });
#pragma warning restore 612, 618
        }
    }
}
