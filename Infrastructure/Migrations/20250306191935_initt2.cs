using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChooseUss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleTj = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DescriptionTj = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TitleRu = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DescriptionRu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IconPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChooseUss", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChooseUss");
        }
    }
}
