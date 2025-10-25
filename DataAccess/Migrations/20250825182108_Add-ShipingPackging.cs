using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShipingPackging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShipingPackgingId",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbShipingPackging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipingPackgingAname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipingPackgingEname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipingPackging", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_ShipingPackgingId",
                table: "TbShippments",
                column: "ShipingPackgingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbShipingPackging_ShipingPackgingId",
                table: "TbShippments",
                column: "ShipingPackgingId",
                principalTable: "TbShipingPackging",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbShipingPackging_ShipingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropTable(
                name: "TbShipingPackging");

            migrationBuilder.DropIndex(
                name: "IX_TbShippments_ShipingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "ShipingPackgingId",
                table: "TbShippments");
        }
    }
}
