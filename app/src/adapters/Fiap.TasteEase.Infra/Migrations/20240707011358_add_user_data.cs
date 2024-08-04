using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.TasteEase.Infra.Migrations
{
    /// <inheritdoc />
    public partial class add_user_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "cellphone_number",
                schema: "taste_ease",
                table: "client",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_address",
                schema: "taste_ease",
                table: "client",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cellphone_number",
                schema: "taste_ease",
                table: "client");

            migrationBuilder.DropColumn(
                name: "full_address",
                schema: "taste_ease",
                table: "client");
        }
    }
}
