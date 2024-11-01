using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrayonAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNameFromSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
