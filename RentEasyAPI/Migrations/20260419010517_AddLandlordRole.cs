using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentEasyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLandlordRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Landlords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Landlords");
        }
    }
}
