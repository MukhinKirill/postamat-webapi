using Microsoft.EntityFrameworkCore.Migrations;

namespace PostamatService.Web.Migrations
{
    public partial class DatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postamats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postamats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostamatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Orders_Postamats_PostamatId",
                        column: x => x.PostamatId,
                        principalTable: "Postamats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInOrder_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Postamats",
                columns: new[] { "Id", "Address", "IsActive", "Number" },
                values: new object[] { 1, "Красная пл., 3, Москва, 109012", true, "1111-222" });

            migrationBuilder.InsertData(
                table: "Postamats",
                columns: new[] { "Id", "Address", "IsActive", "Number" },
                values: new object[] { 2, "ул. Энгельса, 18, Большие Дворы, Московская обл., 142541", true, "1111-111" });

            migrationBuilder.InsertData(
                table: "Postamats",
                columns: new[] { "Id", "Address", "IsActive", "Number" },
                values: new object[] { 3, "Дворцовая пл., 2, Санкт-Петербург, 190000", false, "1111-000" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Number", "Cost", "FullName", "PhoneNumber", "PostamatId", "Status" },
                values: new object[] { 1, 1000.21m, "Ivanov Ivan Ivanovich", "+7999-111-22-33", 1, 1 });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Number", "Cost", "FullName", "PhoneNumber", "PostamatId", "Status" },
                values: new object[] { 2, 2000.21m, "Denisov Ivan Ivanovich", "+7999-222-22-33", 2, 2 });

            migrationBuilder.InsertData(
                table: "ProductInOrder",
                columns: new[] { "Id", "Name", "OrderId" },
                values: new object[,]
                {
                    { 1, "Sony s1", 1 },
                    { 2, "Pony p1", 1 },
                    { 3, "Johny j1", 1 },
                    { 4, "Sony s2", 1 },
                    { 5, "Pony p2", 1 },
                    { 6, "Johny j2", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PostamatId",
                table: "Orders",
                column: "PostamatId");

            migrationBuilder.CreateIndex(
                name: "IX_Postamats_Number",
                table: "Postamats",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInOrder_OrderId",
                table: "ProductInOrder",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductInOrder");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Postamats");
        }
    }
}
