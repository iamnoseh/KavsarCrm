using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initt4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Feedbacks",
                newName: "TextTj");

            migrationBuilder.AddColumn<string>(
                name: "TextEn",
                table: "Feedbacks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextRu",
                table: "Feedbacks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextEn",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "TextRu",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "TextTj",
                table: "Feedbacks",
                newName: "Text");
        }
    }
}
