using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initt6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "News",
                newName: "TitleTj");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "News",
                newName: "ContentTj");

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "News",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ContentEn",
                table: "News",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "News",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "News",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleRu",
                table: "News",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PatternCommentId",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PatternCommentId",
                table: "Comments",
                column: "PatternCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_PatternCommentId",
                table: "Comments",
                column: "PatternCommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_PatternCommentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PatternCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ContentEn",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "News");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "News");

            migrationBuilder.DropColumn(
                name: "TitleRu",
                table: "News");

            migrationBuilder.DropColumn(
                name: "PatternCommentId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "TitleTj",
                table: "News",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ContentTj",
                table: "News",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "News",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
