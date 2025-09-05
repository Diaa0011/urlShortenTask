using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace urlshortener.Migrations
{
    /// <inheritdoc />
    public partial class removeclickCountfrombasetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickCount",
                table: "Urls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClickCount",
                table: "Urls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
