using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkeCookBook.Migrations
{
    /// <inheritdoc />
    public partial class addOneToOneMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    BlockNum = table.Column<int>(type: "int", nullable: false),
                    stName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.userId);
                    table.ForeignKey(
                        name: "FK_addresses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
