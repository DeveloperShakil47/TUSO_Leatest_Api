using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TUSO.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrationfortablenamechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundingAgenciesItems");

            migrationBuilder.DropTable(
                name: "ImplemenentingItems");

            migrationBuilder.CreateTable(
                name: "IncidendtFundingAgencies",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<long>(type: "bigint", nullable: false),
                    FundingAgencyId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidendtFundingAgencies", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_IncidendtFundingAgencies_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incidentImplemenentingPartners",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<long>(type: "bigint", nullable: false),
                    ImplementingId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incidentImplemenentingPartners", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_incidentImplemenentingPartners_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncidendtFundingAgencies_IncidentId",
                table: "IncidendtFundingAgencies",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_incidentImplemenentingPartners_IncidentId",
                table: "incidentImplemenentingPartners",
                column: "IncidentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidendtFundingAgencies");

            migrationBuilder.DropTable(
                name: "incidentImplemenentingPartners");

            migrationBuilder.CreateTable(
                name: "FundingAgenciesItems",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    FundingAgencyId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundingAgenciesItems", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_FundingAgenciesItems_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImplemenentingItems",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    FundingAgencyId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImplemenentingItems", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ImplemenentingItems_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundingAgenciesItems_IncidentId",
                table: "FundingAgenciesItems",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_ImplemenentingItems_IncidentId",
                table: "ImplemenentingItems",
                column: "IncidentId");
        }
    }
}
