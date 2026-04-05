using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICMarketsTest.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockchainSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NetworkId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Payload = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockchainSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainSnapshots_NetworkId",
                table: "BlockchainSnapshots",
                column: "NetworkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockchainSnapshots");
        }
    }
}
