using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBlazorCrmApp.Migrations
{
    /// <inheritdoc />
    public partial class CrmMvpFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ContactId",
                table: "Opportunity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Opportunity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Opportunity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Probability",
                table: "Opportunity",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Opportunity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Opportunity",
                type: "REAL",
                precision: 19,
                scale: 4,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ActivityDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Direction = table.Column<string>(type: "TEXT", nullable: true),
                    ContactId = table.Column<long>(type: "INTEGER", nullable: true),
                    CustomerId = table.Column<long>(type: "INTEGER", nullable: true),
                    LeadId = table.Column<long>(type: "INTEGER", nullable: true),
                    OpportunityId = table.Column<long>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activity_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activity_Lead_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Lead",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activity_Opportunity_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "Opportunity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityTag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagId = table.Column<long>(type: "INTEGER", nullable: true),
                    EntityType = table.Column<string>(type: "TEXT", nullable: true),
                    EntityId = table.Column<long>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opportunity_ContactId",
                table: "Opportunity",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunity_CustomerId",
                table: "Opportunity",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ContactId",
                table: "Activity",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_CustomerId",
                table: "Activity",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_LeadId",
                table: "Activity",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_OpportunityId",
                table: "Activity",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityTag_TagId",
                table: "EntityTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunity_Contact_ContactId",
                table: "Opportunity",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunity_Customer_CustomerId",
                table: "Opportunity",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunity_Contact_ContactId",
                table: "Opportunity");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunity_Customer_CustomerId",
                table: "Opportunity");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "EntityTag");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Opportunity_ContactId",
                table: "Opportunity");

            migrationBuilder.DropIndex(
                name: "IX_Opportunity_CustomerId",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "Probability",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Opportunity");
        }
    }
}
