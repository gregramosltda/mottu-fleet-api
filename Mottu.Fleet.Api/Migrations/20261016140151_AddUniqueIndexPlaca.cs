using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Fleet.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexPlaca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Motos_Placa",
                table: "Motos",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Motos_Placa",
                table: "Motos");
        }
    }
}
