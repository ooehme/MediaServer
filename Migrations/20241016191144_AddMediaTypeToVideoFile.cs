using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaTypeToVideoFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "VideoFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "VideoFiles");
        }
    }
}
