using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBlazorCrmApp.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedCrmModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Customer_CustomerId",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SupportCase",
                newName: "SlaHours");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "SupportCase",
                newName: "ResolvedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "SupportCase",
                newName: "EscalatedDate");

            migrationBuilder.AlterColumn<long>(
                name: "ServiceId",
                table: "SupportCase",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ContactId",
                table: "SupportCase",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "SupportCase",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ServiceId",
                table: "Sale",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "Sale",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Sale",
                type: "TEXT",
                precision: 19,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Sale",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OpportunityId",
                table: "Sale",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Sale",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Sale",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesRepId",
                table: "Sale",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Sale",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "Sale",
                type: "TEXT",
                precision: 19,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Sale",
                type: "TEXT",
                precision: 19,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualCloseDate",
                table: "Opportunity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "Opportunity",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitorInfo",
                table: "Opportunity",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

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
                name: "LostReason",
                table: "Opportunity",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Opportunity",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Probability",
                table: "Opportunity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Opportunity",
                type: "REAL",
                precision: 19,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "Lead",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Lead",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConversionNotes",
                table: "Lead",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConvertedDate",
                table: "Lead",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConvertedToCustomerId",
                table: "Lead",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Lead",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastContactDate",
                table: "Lead",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadScore",
                table: "Lead",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Lead",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customer",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountManagerId",
                table: "Customer",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AnnualRevenue",
                table: "Customer",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeCount",
                table: "Customer",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Customer",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Customer",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Contact",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contact",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId1",
                table: "Contact",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Contact",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DoNotContact",
                table: "Contact",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "Contact",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Contact",
                type: "TEXT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobilePhone",
                table: "Contact",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EntityType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EntityId = table.Column<long>(type: "INTEGER", nullable: true),
                    EntityName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OldValues = table.Column<string>(type: "TEXT", nullable: true),
                    NewValues = table.Column<string>(type: "TEXT", nullable: true),
                    ChangedField = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Communication",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Direction = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ContactId = table.Column<long>(type: "INTEGER", nullable: true),
                    LeadId = table.Column<long>(type: "INTEGER", nullable: true),
                    CustomerId = table.Column<long>(type: "INTEGER", nullable: true),
                    OpportunityId = table.Column<long>(type: "INTEGER", nullable: true),
                    SupportCaseId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    CommunicationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: true),
                    Outcome = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    FollowUpRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communication_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Communication_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Communication_Lead_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Lead",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Communication_Opportunity_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "Opportunity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Communication_SupportCase_SupportCaseId",
                        column: x => x.SupportCaseId,
                        principalTable: "SupportCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportCase_ContactId",
                table: "SupportCase",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportCase_CustomerId",
                table: "SupportCase",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportCase_Priority",
                table: "SupportCase",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_SupportCase_ProductId",
                table: "SupportCase",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportCase_ServiceId",
                table: "SupportCase",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportCase_Status",
                table: "SupportCase",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CustomerId",
                table: "Sale",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_OpportunityId",
                table: "Sale",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ProductId",
                table: "Sale",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_SaleDate",
                table: "Sale",
                column: "SaleDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ServiceId",
                table: "Sale",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunity_AssignedUserId",
                table: "Opportunity",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunity_ContactId",
                table: "Opportunity",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunity_CustomerId",
                table: "Opportunity",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunity_Stage",
                table: "Opportunity",
                column: "Stage");

            migrationBuilder.CreateIndex(
                name: "IX_Lead_AssignedUserId",
                table: "Lead",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lead_ConvertedToCustomerId",
                table: "Lead",
                column: "ConvertedToCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lead_Status",
                table: "Lead",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CustomerId1",
                table: "Contact",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_EntityId",
                table: "ActivityLog",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_EntityType",
                table: "ActivityLog",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_Timestamp",
                table: "ActivityLog",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_UserId",
                table: "ActivityLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_CommunicationDate",
                table: "Communication",
                column: "CommunicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_ContactId",
                table: "Communication",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_CustomerId",
                table: "Communication",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_LeadId",
                table: "Communication",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_OpportunityId",
                table: "Communication",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_SupportCaseId",
                table: "Communication",
                column: "SupportCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_Type",
                table: "Communication",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Customer_CustomerId",
                table: "Contact",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Customer_CustomerId1",
                table: "Contact",
                column: "CustomerId1",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lead_Customer_ConvertedToCustomerId",
                table: "Lead",
                column: "ConvertedToCustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunity_Contact_ContactId",
                table: "Opportunity",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunity_Customer_CustomerId",
                table: "Opportunity",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Customer_CustomerId",
                table: "Sale",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Opportunity_OpportunityId",
                table: "Sale",
                column: "OpportunityId",
                principalTable: "Opportunity",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Product_ProductId",
                table: "Sale",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Service_ServiceId",
                table: "Sale",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportCase_Contact_ContactId",
                table: "SupportCase",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportCase_Customer_CustomerId",
                table: "SupportCase",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportCase_Product_ProductId",
                table: "SupportCase",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportCase_Service_ServiceId",
                table: "SupportCase",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Customer_CustomerId",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Customer_CustomerId1",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Lead_Customer_ConvertedToCustomerId",
                table: "Lead");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunity_Contact_ContactId",
                table: "Opportunity");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunity_Customer_CustomerId",
                table: "Opportunity");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Customer_CustomerId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Opportunity_OpportunityId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Product_ProductId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Service_ServiceId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportCase_Contact_ContactId",
                table: "SupportCase");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportCase_Customer_CustomerId",
                table: "SupportCase");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportCase_Product_ProductId",
                table: "SupportCase");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportCase_Service_ServiceId",
                table: "SupportCase");

            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "Communication");

            migrationBuilder.DropIndex(
                name: "IX_SupportCase_ContactId",
                table: "SupportCase");

            migrationBuilder.DropIndex(
                name: "IX_SupportCase_CustomerId",
                table: "SupportCase");

            migrationBuilder.DropIndex(
                name: "IX_SupportCase_Priority",
                table: "SupportCase");

            migrationBuilder.DropIndex(
                name: "IX_SupportCase_ProductId",
                table: "SupportCase");

            migrationBuilder.DropIndex(
                name: "IX_SupportCase_ServiceId",
                table: "SupportCase");

            migrationBuilder.DropIndex(
                name: "IX_SupportCase_Status",
                table: "SupportCase");

            migrationBuilder.DropIndex(
                name: "IX_Sale_CustomerId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_OpportunityId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_ProductId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_SaleDate",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_ServiceId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Opportunity_AssignedUserId",
                table: "Opportunity");

            migrationBuilder.DropIndex(
                name: "IX_Opportunity_ContactId",
                table: "Opportunity");

            migrationBuilder.DropIndex(
                name: "IX_Opportunity_CustomerId",
                table: "Opportunity");

            migrationBuilder.DropIndex(
                name: "IX_Opportunity_Stage",
                table: "Opportunity");

            migrationBuilder.DropIndex(
                name: "IX_Lead_AssignedUserId",
                table: "Lead");

            migrationBuilder.DropIndex(
                name: "IX_Lead_ConvertedToCustomerId",
                table: "Lead");

            migrationBuilder.DropIndex(
                name: "IX_Lead_Status",
                table: "Lead");

            migrationBuilder.DropIndex(
                name: "IX_Contact_CustomerId1",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "Channel",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "OpportunityId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "SalesRepId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "ActualCloseDate",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "CompetitorInfo",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "LostReason",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "Probability",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "ConversionNotes",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "ConvertedDate",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "ConvertedToCustomerId",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "LastContactDate",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "LeadScore",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "AccountManagerId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "AnnualRevenue",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "EmployeeCount",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "DoNotContact",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "MobilePhone",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "SlaHours",
                table: "SupportCase",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ResolvedDate",
                table: "SupportCase",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "EscalatedDate",
                table: "SupportCase",
                newName: "CreatedDateTime");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceId",
                table: "SupportCase",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceId",
                table: "Sale",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Sale",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customer",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<long>(
                name: "Phone",
                table: "Contact",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contact",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Customer_CustomerId",
                table: "Contact",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
