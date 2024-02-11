using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSpace.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddHasLikedColumnToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasLiked",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasLiked",
                table: "Posts");
        }
    }
}
