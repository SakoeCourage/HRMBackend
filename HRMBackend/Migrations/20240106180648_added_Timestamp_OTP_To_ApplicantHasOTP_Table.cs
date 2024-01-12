using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMBackend.Migrations
{
    /// <inheritdoc />
    public partial class added_Timestamp_OTP_To_ApplicantHasOTP_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantHasToken_Applicant_applicantID",
                table: "ApplicantHasToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicantHasToken",
                table: "ApplicantHasToken");

            migrationBuilder.RenameTable(
                name: "ApplicantHasToken",
                newName: "ApplicantHasOTP");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantHasToken_applicantID",
                table: "ApplicantHasOTP",
                newName: "IX_ApplicantHasOTP_applicantID");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "ApplicantHasOTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "ApplicantHasOTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicantHasOTP",
                table: "ApplicantHasOTP",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantHasOTP_Applicant_applicantID",
                table: "ApplicantHasOTP",
                column: "applicantID",
                principalTable: "Applicant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantHasOTP_Applicant_applicantID",
                table: "ApplicantHasOTP");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicantHasOTP",
                table: "ApplicantHasOTP");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "ApplicantHasOTP");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "ApplicantHasOTP");

            migrationBuilder.RenameTable(
                name: "ApplicantHasOTP",
                newName: "ApplicantHasToken");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantHasOTP_applicantID",
                table: "ApplicantHasToken",
                newName: "IX_ApplicantHasToken_applicantID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicantHasToken",
                table: "ApplicantHasToken",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantHasToken_Applicant_applicantID",
                table: "ApplicantHasToken",
                column: "applicantID",
                principalTable: "Applicant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
