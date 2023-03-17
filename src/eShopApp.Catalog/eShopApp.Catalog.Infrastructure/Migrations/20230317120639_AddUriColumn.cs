using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopApp.Catalog.Infrastructure.Migrations
{
    public partial class AddUriColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconUri",
                schema: "Catalog",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUri",
                schema: "Catalog",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUri",
                schema: "Catalog",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "PictureUri",
                schema: "Catalog",
                table: "Brand");
        }
    }
}
