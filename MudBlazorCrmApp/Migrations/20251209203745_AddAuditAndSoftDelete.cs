using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBlazorCrmApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditAndSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "SupportCase",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SupportCase",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "SupportCase",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Sale",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Sale",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Sale",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sale",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Sale",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Opportunity",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Opportunity",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Opportunity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Opportunity",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Opportunity",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Lead",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Lead",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Lead",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lead",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Lead",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customer",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Customer",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Customer",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customer",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Customer",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Contact",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Contact",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Contact",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Contact",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Contact",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Communication",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Communication",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Communication",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Communication",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Communication",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Communication",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EntityType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ChangeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TableName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    OldValues = table.Column<string>(type: "TEXT", nullable: true),
                    NewValues = table.Column<string>(type: "TEXT", nullable: true),
                    ChangedProperties = table.Column<string>(type: "TEXT", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AdditionalInfo = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CorrelationId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CorrelationId",
                table: "AuditLog",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityId",
                table: "AuditLog",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityType",
                table: "AuditLog",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Timestamp",
                table: "AuditLog",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_UserId",
                table: "AuditLog",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SupportCase");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Communication");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Communication");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Communication");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Communication");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Communication");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Communication",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
