using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentEasyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRelations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tenants",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Properties",
                newName: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Tenants",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Properties",
                newName: "Id");
        }
    }
}
