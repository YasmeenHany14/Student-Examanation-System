using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalScoreColumnToExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinalScore",
                table: "GeneratedExams",
                newName: "StudentScore");

            migrationBuilder.AddColumn<int>(
                name: "ExamTotalScore",
                table: "GeneratedExams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamTotalScore",
                table: "GeneratedExams");

            migrationBuilder.RenameColumn(
                name: "StudentScore",
                table: "GeneratedExams",
                newName: "FinalScore");
        }
    }
}
