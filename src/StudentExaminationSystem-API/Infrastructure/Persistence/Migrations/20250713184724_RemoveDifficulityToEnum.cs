using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDifficulityToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DifficultyProfiles_Difficulties_DifficultyId",
                table: "DifficultyProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Difficulties_DifficultyId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "DifficultyProfileConfigs");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropIndex(
                name: "IX_Questions_DifficultyId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_DifficultyProfiles_DifficultyId",
                table: "DifficultyProfiles");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "DifficultyProfiles");

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EasyPercentage",
                table: "DifficultyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HardPercentage",
                table: "DifficultyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediumPercentage",
                table: "DifficultyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "EasyPercentage",
                table: "DifficultyProfiles");

            migrationBuilder.DropColumn(
                name: "HardPercentage",
                table: "DifficultyProfiles");

            migrationBuilder.DropColumn(
                name: "MediumPercentage",
                table: "DifficultyProfiles");

            migrationBuilder.AddColumn<int>(
                name: "DifficultyId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyId",
                table: "DifficultyProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DifficultyId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DifficultyProfileConfigs",
                columns: table => new
                {
                    DifficultyProfileId = table.Column<int>(type: "int", nullable: false),
                    DifficultyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DifficultyPercentage = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultyProfileConfigs", x => new { x.DifficultyProfileId, x.DifficultyId });
                    table.ForeignKey(
                        name: "FK_DifficultyProfileConfigs_Difficulties_DifficultyId",
                        column: x => x.DifficultyId,
                        principalTable: "Difficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DifficultyProfileConfigs_DifficultyProfiles_DifficultyProfileId",
                        column: x => x.DifficultyProfileId,
                        principalTable: "DifficultyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DifficultyId",
                table: "Questions",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_DifficultyProfiles_DifficultyId",
                table: "DifficultyProfiles",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_DifficultyProfileConfigs_DifficultyId",
                table: "DifficultyProfileConfigs",
                column: "DifficultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DifficultyProfiles_Difficulties_DifficultyId",
                table: "DifficultyProfiles",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Difficulties_DifficultyId",
                table: "Questions",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
