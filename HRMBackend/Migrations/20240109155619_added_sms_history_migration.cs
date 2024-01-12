using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMBackend.Migrations
{
    /// <inheritdoc />
    public partial class added_sms_history_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMSHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    campaignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    smsTemplateId = table.Column<int>(type: "int", nullable: false),
                    receipients = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_SMSHistory_SMSTemplate_smsTemplateId",
                        column: x => x.smsTemplateId,
                        principalTable: "SMSTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMSHistory_smsTemplateId",
                table: "SMSHistory",
                column: "smsTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSHistory");
        }
    }
}
