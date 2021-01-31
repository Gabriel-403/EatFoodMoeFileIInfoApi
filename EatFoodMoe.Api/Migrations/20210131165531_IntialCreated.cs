using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EatFoodMoe.Api.Migrations
{
    public partial class IntialCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SinicizationGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nmae = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinicizationGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    GameName = table.Column<string>(type: "TEXT", nullable: true),
                    MemberNames = table.Column<string>(type: "TEXT", nullable: true),
                    FileCount = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_SinicizationGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "SinicizationGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EatFoodFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    Translation = table.Column<bool>(type: "INTEGER", nullable: false),
                    Embellishment = table.Column<bool>(type: "INTEGER", nullable: false),
                    Proofreading = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    LastModityTIme = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EatFoodFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EatFoodFiles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SinicizationGroup",
                columns: new[] { "Id", "Nmae" },
                values: new object[] { new Guid("de04d32c-5f8b-4e24-95a4-40855d35a663"), "Unknowed" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "FileCount", "GameName", "GroupId", "MemberNames", "Name" },
                values: new object[] { new Guid("4e43a044-2e4d-4a60-862f-606c5cbdf066"), 0, "Unknowed", new Guid("de04d32c-5f8b-4e24-95a4-40855d35a663"), "Unknowed", "Unknowed" });

            migrationBuilder.CreateIndex(
                name: "IX_EatFoodFiles_ProjectId",
                table: "EatFoodFiles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_GroupId",
                table: "Projects",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EatFoodFiles");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "SinicizationGroup");
        }
    }
}
