using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ManageOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Manages",
                table: "Manages");

            migrationBuilder.RenameTable(
                name: "Manages",
                newName: "ManageOrders");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "ManageOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ManageOrders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManageOrders",
                table: "ManageOrders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ManageOrders_ApplicationUserId",
                table: "ManageOrders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManageOrders_AspNetUsers_ApplicationUserId",
                table: "ManageOrders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManageOrders_AspNetUsers_ApplicationUserId",
                table: "ManageOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManageOrders",
                table: "ManageOrders");

            migrationBuilder.DropIndex(
                name: "IX_ManageOrders_ApplicationUserId",
                table: "ManageOrders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ManageOrders");

            migrationBuilder.RenameTable(
                name: "ManageOrders",
                newName: "Manages");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Manages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manages",
                table: "Manages",
                column: "Id");
        }
    }
}
