using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace forum.Migrations
{
    /// <inheritdoc />
    public partial class updateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accepted",
                table: "friend_request");

            migrationBuilder.AddColumn<string>(
                name: "profile",
                table: "user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "friend_request",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile",
                table: "user");

            migrationBuilder.DropColumn(
                name: "status",
                table: "friend_request");

            migrationBuilder.AddColumn<bool>(
                name: "accepted",
                table: "friend_request",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
