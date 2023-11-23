using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Botris.Migrations
{
    public partial class v12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Usuarios",
                newName: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Usuarios",
                newName: "Id");
        }
    }
}
