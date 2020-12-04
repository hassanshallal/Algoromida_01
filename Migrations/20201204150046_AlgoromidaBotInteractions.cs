using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Algoromida_01.Migrations
{
    public partial class AlgoromidaBotInteractions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    BotId = table.Column<string>(nullable: false),
                    UserQuery = table.Column<string>(nullable: false),
                    InitiatedAt = table.Column<DateTimeOffset>(nullable: false),
                    BotResponse = table.Column<string>(nullable: false),
                    BotAwareness = table.Column<string>(nullable: false),
                    BotStatefulness = table.Column<string>(nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(nullable: false),
                    IsComplete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interactions");
        }
    }
}
