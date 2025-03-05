using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Banners",
                newName: "TitleTj");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Banners",
                newName: "TitleRu");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Banners",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "Banners",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTj",
                table: "Banners",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Banners",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "DescriptionTj",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Banners");

            migrationBuilder.RenameColumn(
                name: "TitleTj",
                table: "Banners",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleRu",
                table: "Banners",
                newName: "Description");
        }
    }
}
