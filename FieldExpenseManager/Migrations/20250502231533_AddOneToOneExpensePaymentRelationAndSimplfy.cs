using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldExpenseManager.Migrations
{
    /// <inheritdoc />
    public partial class AddOneToOneExpensePaymentRelationAndSimplfy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ApproverUserId",
                table: "Expenses",
                column: "ApproverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_ApproverUserId",
                table: "Expenses",
                column: "ApproverUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_ApproverUserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ApproverUserId",
                table: "Expenses");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Expenses",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
