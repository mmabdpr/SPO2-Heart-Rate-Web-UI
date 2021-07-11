using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PulseOM.Data.Migrations
{
    public partial class AddPulseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PulseData",
                columns: table => new
                {
                    PulseDataItemId = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HeartBeat = table.Column<long>(type: "INTEGER", nullable: false),
                    Oxygen = table.Column<long>(type: "INTEGER", nullable: false),
                    IdentityUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PulseData", x => x.PulseDataItemId);
                    table.ForeignKey(
                        name: "FK_PulseData_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PulseData_IdentityUserId",
                table: "PulseData",
                column: "IdentityUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PulseData");
        }
    }
}
