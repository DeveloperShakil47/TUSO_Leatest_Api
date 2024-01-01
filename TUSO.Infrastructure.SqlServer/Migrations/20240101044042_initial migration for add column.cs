using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TUSO.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrationforaddcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsITExpart",
                table: "FacilityPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsITExpart",
                table: "FacilityPermissions");
        }
    }
}
