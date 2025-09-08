using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkeCookBook.Migrations
{
    /// <inheritdoc />
    public partial class timeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "rowVersion",
                table: "Users",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rowVersion",
                table: "Users");
        }
    }
}
