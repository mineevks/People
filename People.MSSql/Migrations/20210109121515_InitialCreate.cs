using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace People.MSSql.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Patronymic = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Snils = table.Column<long>(type: "bigint", nullable: true),
                    Inn = table.Column<long>(type: "bigint", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfDeath = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.Guid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_DateOfBirth",
                table: "Citizens",
                column: "DateOfBirth");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_DateOfDeath",
                table: "Citizens",
                column: "DateOfDeath");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_Inn",
                table: "Citizens",
                column: "Inn");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_Name",
                table: "Citizens",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_Patronymic",
                table: "Citizens",
                column: "Patronymic");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_Snils",
                table: "Citizens",
                column: "Snils");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_Surname",
                table: "Citizens",
                column: "Surname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citizens");
        }
    }
}
