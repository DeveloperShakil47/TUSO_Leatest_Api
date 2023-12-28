using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TUSO.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrationfornewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FundingAgenciesItems_FundingAgencies_FundingAgencyId",
                table: "FundingAgenciesItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ImplemenentingItems_FundingAgencies_FundingAgencyId",
                table: "ImplemenentingItems");

            migrationBuilder.DropIndex(
                name: "IX_ImplemenentingItems_FundingAgencyId",
                table: "ImplemenentingItems");

            migrationBuilder.DropIndex(
                name: "IX_FundingAgenciesItems_FundingAgencyId",
                table: "FundingAgenciesItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ImplemenentingItems_FundingAgencyId",
                table: "ImplemenentingItems",
                column: "FundingAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FundingAgenciesItems_FundingAgencyId",
                table: "FundingAgenciesItems",
                column: "FundingAgencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FundingAgenciesItems_FundingAgencies_FundingAgencyId",
                table: "FundingAgenciesItems",
                column: "FundingAgencyId",
                principalTable: "FundingAgencies",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImplemenentingItems_FundingAgencies_FundingAgencyId",
                table: "ImplemenentingItems",
                column: "FundingAgencyId",
                principalTable: "FundingAgencies",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
