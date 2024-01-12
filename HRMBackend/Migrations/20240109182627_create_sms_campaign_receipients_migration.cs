using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMBackend.Migrations
{
    /// <inheritdoc />
    public partial class create_sms_campaign_receipients_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSHistory");

            migrationBuilder.CreateTable(
                name: "SMSCampaignHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    campaignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    smsTemplateId = table.Column<int>(type: "int", nullable: false),
                    receipients = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSCampaignHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_SMSCampaignHistory_SMSTemplate_smsTemplateId",
                        column: x => x.smsTemplateId,
                        principalTable: "SMSTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SMSCampaignReceipient",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    campaignHistoryId = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Status => Pending or Successful or Failed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSCampaignReceipient", x => x.id);
                    table.ForeignKey(
                        name: "FK_SMSCampaignReceipient_SMSCampaignHistory_campaignHistoryId",
                        column: x => x.campaignHistoryId,
                        principalTable: "SMSCampaignHistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMSCampaignHistory_smsTemplateId",
                table: "SMSCampaignHistory",
                column: "smsTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SMSCampaignReceipient_campaignHistoryId",
                table: "SMSCampaignReceipient",
                column: "campaignHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSCampaignReceipient");

            migrationBuilder.DropTable(
                name: "SMSCampaignHistory");

            migrationBuilder.CreateTable(
                name: "SMSHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    smsTemplateId = table.Column<int>(type: "int", nullable: false),
                    campaignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
    }
}
