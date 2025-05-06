using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldExpenseManager.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureExpensePayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentses_Expenses_ExpenseId",
                table: "ExpensePaymentses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensePaymentses",
                table: "ExpensePaymentses");

            migrationBuilder.DropIndex(
                name: "IX_ExpensePaymentses_ExpenseId",
                table: "ExpensePaymentses");

            migrationBuilder.RenameTable(
                name: "ExpensePaymentses",
                newName: "ExpensePayments");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "ExpensePayments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ExpensePayments",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "ExpensePayments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ExpensePayments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensePayments",
                table: "ExpensePayments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_ExpenseId",
                table: "ExpensePayments",
                column: "ExpenseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayments_Expenses_ExpenseId",
                table: "ExpensePayments",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayments_Expenses_ExpenseId",
                table: "ExpensePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensePayments",
                table: "ExpensePayments");

            migrationBuilder.DropIndex(
                name: "IX_ExpensePayments_ExpenseId",
                table: "ExpensePayments");

            migrationBuilder.RenameTable(
                name: "ExpensePayments",
                newName: "ExpensePaymentses");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "ExpensePaymentses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ExpensePaymentses",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "ExpensePaymentses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ExpensePaymentses",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensePaymentses",
                table: "ExpensePaymentses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentses_ExpenseId",
                table: "ExpensePaymentses",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentses_Expenses_ExpenseId",
                table: "ExpensePaymentses",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
