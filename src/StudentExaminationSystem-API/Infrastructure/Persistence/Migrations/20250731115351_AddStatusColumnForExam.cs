using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumnForExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "GeneratedExams");

            migrationBuilder.AddColumn<int>(
                name: "ExamStatus",
                table: "GeneratedExams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamStatus",
                table: "GeneratedExams");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "GeneratedExams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
