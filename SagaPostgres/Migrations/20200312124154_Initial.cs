using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaPostgres.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServeBeerState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    CurrentState = table.Column<string>(maxLength: 255, nullable: false),
                    BeerType = table.Column<string>(maxLength: 255, nullable: true),
                    Tip = table.Column<decimal>(nullable: false),
                    PaidAndServedStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServeBeerState", x => x.CorrelationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServeBeerState_OrderId",
                table: "ServeBeerState",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServeBeerState");
        }
    }
}
