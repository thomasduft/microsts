using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tomware.Microsts.Web.Data.Migrations
{
    public partial class ClaimTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimType_Name",
                table: "ClaimType",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimType");
        }
    }
}
