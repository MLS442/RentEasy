using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentEasyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TenantId",
                table: "Tickets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_PropertyId",
                table: "Tenants",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Properties_PropertyId",
                table: "Tenants",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Tenants_TenantId",
                table: "Tickets",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Properties_PropertyId",
                table: "Tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Tenants_TenantId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TenantId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_PropertyId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Tenants");
        }
    }
}
