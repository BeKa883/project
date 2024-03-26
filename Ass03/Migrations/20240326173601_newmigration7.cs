using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ass03.Migrations
{
    /// <inheritdoc />
    public partial class newmigration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderBill",
                table: "OrderBill");

            migrationBuilder.RenameTable(
                name: "OrderBill",
                newName: "OrderBills");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderBills",
                table: "OrderBills",
                column: "OrderBillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderBills",
                table: "OrderBills");

            migrationBuilder.RenameTable(
                name: "OrderBills",
                newName: "OrderBill");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderBill",
                table: "OrderBill",
                column: "OrderBillId");
        }
    }
}
