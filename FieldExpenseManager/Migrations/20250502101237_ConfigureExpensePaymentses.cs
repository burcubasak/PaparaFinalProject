using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldExpenseManager.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureExpensePaymentses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayments_Expenses_ExpenseId",
                table: "ExpensePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensePayments",
                table: "ExpensePayments");

            migrationBuilder.RenameTable(
                name: "ExpensePayments",
                newName: "ExpensePaymentses");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensePayments_ExpenseId",
                table: "ExpensePaymentses",
                newName: "IX_ExpensePaymentses_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensePaymentses",
                table: "ExpensePaymentses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentses_Expenses_ExpenseId",
                table: "ExpensePaymentses",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentses_Expenses_ExpenseId",
                table: "ExpensePaymentses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensePaymentses",
                table: "ExpensePaymentses");

            migrationBuilder.RenameTable(
                name: "ExpensePaymentses",
                newName: "ExpensePayments");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensePaymentses_ExpenseId",
                table: "ExpensePayments",
                newName: "IX_ExpensePayments_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensePayments",
                table: "ExpensePayments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayments_Expenses_ExpenseId",
                table: "ExpensePayments",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
