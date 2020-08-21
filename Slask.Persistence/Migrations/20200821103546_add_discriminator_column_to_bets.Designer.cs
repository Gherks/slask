﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Slask.Domain.Utilities;
using Slask.Persistence;

namespace Slask.Persistence.Migrations
{
    [DbContext(typeof(SlaskContext))]
    [Migration("20200821103546_add_discriminator_column_to_bets")]
    partial class add_discriminator_column_to_bets
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Slask.Domain.Bets.BetBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BetType")
                        .HasColumnType("int");

                    b.Property<Guid>("BetterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BetterId");

                    b.ToTable("Bet");

                    b.HasDiscriminator<int>("BetType").HasValue(0);
                });

            modelBuilder.Entity("Slask.Domain.Better", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.HasIndex("UserId");

                    b.ToTable("Better");
                });

            modelBuilder.Entity("Slask.Domain.Groups.GroupBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ContestType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoundId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoundId");

                    b.ToTable("Group");

                    b.HasDiscriminator<int>("ContestType").HasValue(0);
                });

            modelBuilder.Entity("Slask.Domain.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BestOf")
                        .HasColumnType("int");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Player1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Player2Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("Player1Id")
                        .IsUnique();

                    b.HasIndex("Player2Id")
                        .IsUnique();

                    b.ToTable("Match");
                });

            modelBuilder.Entity("Slask.Domain.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerReferenceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("Slask.Domain.PlayerReference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RoundBaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoundBaseId");

                    b.HasIndex("TournamentId");

                    b.ToTable("PlayerReference");
                });

            modelBuilder.Entity("Slask.Domain.Rounds.RoundBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AdvancingPerGroupCount")
                        .HasColumnType("int");

                    b.Property<int>("ContestType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayersPerGroupCount")
                        .HasColumnType("int");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Round");

                    b.HasDiscriminator<int>("ContestType").HasValue(0);
                });

            modelBuilder.Entity("Slask.Domain.Tournament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Slask.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Slask.Domain.Bets.BetTypes.MatchBet", b =>
                {
                    b.HasBaseType("Slask.Domain.Bets.BetBase");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Slask.Domain.Bets.BetTypes.MiscellaneousBet", b =>
                {
                    b.HasBaseType("Slask.Domain.Bets.BetBase");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Slask.Domain.Groups.GroupTypes.BracketGroup", b =>
                {
                    b.HasBaseType("Slask.Domain.Groups.GroupBase");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Slask.Domain.Groups.GroupTypes.DualTournamentGroup", b =>
                {
                    b.HasBaseType("Slask.Domain.Groups.GroupBase");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Slask.Domain.Groups.GroupTypes.RoundRobinGroup", b =>
                {
                    b.HasBaseType("Slask.Domain.Groups.GroupBase");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Slask.Domain.Rounds.RoundTypes.BracketRound", b =>
                {
                    b.HasBaseType("Slask.Domain.Rounds.RoundBase");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Slask.Domain.Rounds.RoundTypes.DualTournamentRound", b =>
                {
                    b.HasBaseType("Slask.Domain.Rounds.RoundBase");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Slask.Domain.Rounds.RoundTypes.RoundRobinRound", b =>
                {
                    b.HasBaseType("Slask.Domain.Rounds.RoundBase");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Slask.Domain.Bets.BetBase", b =>
                {
                    b.HasOne("Slask.Domain.Better", "Better")
                        .WithMany("Bets")
                        .HasForeignKey("BetterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Slask.Domain.Better", b =>
                {
                    b.HasOne("Slask.Domain.Tournament", "Tournament")
                        .WithMany("Betters")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Slask.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Slask.Domain.Groups.GroupBase", b =>
                {
                    b.HasOne("Slask.Domain.Rounds.RoundBase", "Round")
                        .WithMany("Groups")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Slask.Domain.Match", b =>
                {
                    b.HasOne("Slask.Domain.Groups.GroupBase", "Group")
                        .WithMany("Matches")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Slask.Domain.Player", "Player1")
                        .WithOne()
                        .HasForeignKey("Slask.Domain.Match", "Player1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Slask.Domain.Player", "Player2")
                        .WithOne()
                        .HasForeignKey("Slask.Domain.Match", "Player2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Slask.Domain.Player", b =>
                {
                    b.HasOne("Slask.Domain.Match", "Match")
                        .WithMany()
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Slask.Domain.PlayerReference", b =>
                {
                    b.HasOne("Slask.Domain.Rounds.RoundBase", null)
                        .WithMany("PlayerReferences")
                        .HasForeignKey("RoundBaseId");

                    b.HasOne("Slask.Domain.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Slask.Domain.Rounds.RoundBase", b =>
                {
                    b.HasOne("Slask.Domain.Tournament", "Tournament")
                        .WithMany("Rounds")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
