﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using buildeR.DAL.Context;

namespace buildeR.DAL.Migrations
{
    [DbContext(typeof(BuilderContext))]
    [Migration("20200823180200_ChangeNotificationSettings")]
    partial class ChangeNotificationSettings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("buildeR.DAL.Entities.BuildHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BranchHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BuildAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("BuildStatus")
                        .HasColumnType("int");

                    b.Property<string>("CommitHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("PerformerId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PerformerId");

                    b.HasIndex("ProjectId");

                    b.ToTable("BuildHistories");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.BuildPlugin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Command")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DockerImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PluginName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BuildPlugins");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Command = "dotnet",
                            DockerImageName = "mcr.microsoft.com/dotnet/core/sdk",
                            PluginName = ".NET Core"
                        },
                        new
                        {
                            Id = 2,
                            Command = "npm",
                            DockerImageName = "node",
                            PluginName = "Node.js"
                        });
                });

            modelBuilder.Entity("buildeR.DAL.Entities.BuildPluginParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuildStepId")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BuildStepId");

                    b.ToTable("BuildPluginParamemeters");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.BuildStep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BuildStepName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("LoggingVerbosity")
                        .HasColumnType("int");

                    b.Property<int>("PluginCommandId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("WorkDirectory")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PluginCommandId");

                    b.HasIndex("ProjectId");

                    b.ToTable("BuildSteps");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<int>("EntityType")
                        .HasColumnType("int");

                    b.Property<string>("NotificationMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NotificationTrigger")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.NotificationSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("EnableApp")
                        .HasColumnType("bit");

                    b.Property<bool>("EnableEmail")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("NotificationSettings");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.NotificationSettingOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("App")
                        .HasColumnType("bit");

                    b.Property<bool>("Email")
                        .HasColumnType("bit");

                    b.Property<int>("NotificationSettingId")
                        .HasColumnType("int");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotificationSettingId");

                    b.ToTable("NotificationSettingOptions");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.PluginCommand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PluginId")
                        .HasColumnType("int");

                    b.Property<string>("TemplateForDocker")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PluginId");

                    b.ToTable("PluginCommands");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "restore",
                            PluginId = 1
                        },
                        new
                        {
                            Id = 1,
                            Name = "build",
                            PluginId = 1
                        },
                        new
                        {
                            Id = 3,
                            Name = "install",
                            PluginId = 2
                        });
                });

            modelBuilder.Entity("buildeR.DAL.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CancelAfter")
                        .HasColumnType("int");

                    b.Property<string>("CredentialsId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAutoCancelBranchBuilds")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAutoCancelPullRequestBuilds")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCleanUpBeforeBuild")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Repository")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.ProjectGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectGroups");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.ProjectTrigger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BranchHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("TriggerType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectTriggers");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.Repository", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Privte")
                        .HasColumnType("bit");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.TeamMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("MemberRole")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("TeamMembers");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.UserSocialNetwork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProviderName")
                        .HasColumnType("int");

                    b.Property<string>("SocialNetworkUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSocialNetworks");
                });

            modelBuilder.Entity("buildeR.DAL.Entities.BuildHistory", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.User", "Performer")
                        .WithMany("BuildHistories")
                        .HasForeignKey("PerformerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("buildeR.DAL.Entities.Project", "Project")
                        .WithMany("BuildHistories")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.BuildPluginParameter", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.BuildStep", "BuildStep")
                        .WithMany("BuildPluginParameters")
                        .HasForeignKey("BuildStepId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.BuildStep", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.PluginCommand", "PluginCommand")
                        .WithMany("BuildSteps")
                        .HasForeignKey("PluginCommandId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("buildeR.DAL.Entities.Project", "Project")
                        .WithMany("BuildSteps")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.NotificationSetting", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.User", "User")
                        .WithOne("NotificationSetting")
                        .HasForeignKey("buildeR.DAL.Entities.NotificationSetting", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.NotificationSettingOption", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.NotificationSetting", "NotificationSetting")
                        .WithMany("NotificationSettingOptions")
                        .HasForeignKey("NotificationSettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.PluginCommand", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.BuildPlugin", "Plugin")
                        .WithMany("PluginCommands")
                        .HasForeignKey("PluginId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.Project", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.User", "Owner")
                        .WithMany("Projects")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.ProjectGroup", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.Group", "Group")
                        .WithMany("ProjectGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("buildeR.DAL.Entities.Project", "Project")
                        .WithMany("ProjectGroups")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.ProjectTrigger", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.Project", "Project")
                        .WithMany("ProjectTriggers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.Repository", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.Project", "Project")
                        .WithOne("_Repository")
                        .HasForeignKey("buildeR.DAL.Entities.Repository", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.TeamMember", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.Group", "Group")
                        .WithMany("TeamMembers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("buildeR.DAL.Entities.User", "User")
                        .WithMany("TeamMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("buildeR.DAL.Entities.UserSocialNetwork", b =>
                {
                    b.HasOne("buildeR.DAL.Entities.User", "User")
                        .WithMany("UserSocialNetworks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
