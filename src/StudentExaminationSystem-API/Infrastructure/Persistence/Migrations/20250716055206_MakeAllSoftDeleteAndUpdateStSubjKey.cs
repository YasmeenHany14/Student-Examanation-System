using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeAllSoftDeleteAndUpdateStSubjKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SubjectExamConfigs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubjectExamConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudentSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StudentSubjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentSubjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId1",
                table: "StudentSubjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "GeneratedExams",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GeneratedExams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GeneratedExams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionChoiceId",
                table: "AnswerHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AnswerHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AnswerHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_StudentId_SubjectId",
                table: "StudentSubjects",
                columns: new[] { "StudentId", "SubjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_SubjectId1",
                table: "StudentSubjects",
                column: "SubjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerHistories_GeneratedExams_GeneratedExamId",
                table: "AnswerHistories",
                column: "GeneratedExamId",
                principalTable: "GeneratedExams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId1",
                table: "StudentSubjects",
                column: "SubjectId1",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerHistories_GeneratedExams_GeneratedExamId",
                table: "AnswerHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId1",
                table: "StudentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjects_StudentId_SubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjects_SubjectId1",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SubjectExamConfigs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubjectExamConfigs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "SubjectId1",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "GeneratedExams");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GeneratedExams");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AnswerHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AnswerHistories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "GeneratedExams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionChoiceId",
                table: "AnswerHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects",
                columns: new[] { "StudentId", "SubjectId" });
        }
    }
}
