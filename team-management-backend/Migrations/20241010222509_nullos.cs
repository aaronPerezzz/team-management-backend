using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace team_management_backend.Migrations
{
    /// <inheritdoc />
    public partial class nullos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AñoCompra",
                table: "CaracteristicasTransportes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AñoCompra",
                table: "CaracteristicasTransportes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
