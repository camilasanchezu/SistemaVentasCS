using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaVenta.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SecondInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idUsuario",
                table: "DetalleVenta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVenta_idUsuario",
                table: "DetalleVenta",
                column: "idUsuario",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__DetalleVe__idUsu__7A672E12",
                table: "DetalleVenta",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DetalleVe__idUsu__7A672E12",
                table: "DetalleVenta");

            migrationBuilder.DropIndex(
                name: "IX_DetalleVenta_idUsuario",
                table: "DetalleVenta");

            migrationBuilder.DropColumn(
                name: "idUsuario",
                table: "DetalleVenta");
        }
    }
}
