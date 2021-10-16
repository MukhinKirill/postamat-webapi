using Microsoft.EntityFrameworkCore.Migrations;

namespace PostamatService.Web.Migrations
{
    public partial class UpdateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductInOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ProductInOrder",
                keyColumn: "Id",
                keyValue: 4,
                column: "OrderId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductInOrder",
                keyColumn: "Id",
                keyValue: 5,
                column: "OrderId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductInOrder",
                keyColumn: "Id",
                keyValue: 6,
                column: "OrderId",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductInOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "ProductInOrder",
                keyColumn: "Id",
                keyValue: 4,
                column: "OrderId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductInOrder",
                keyColumn: "Id",
                keyValue: 5,
                column: "OrderId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductInOrder",
                keyColumn: "Id",
                keyValue: 6,
                column: "OrderId",
                value: 1);
        }
    }
}
