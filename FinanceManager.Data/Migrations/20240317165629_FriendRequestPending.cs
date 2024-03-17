using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class FriendRequestPending : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "Friendship",
                newName: "IsPending");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Friendship",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Friendship");

            migrationBuilder.RenameColumn(
                name: "IsPending",
                table: "Friendship",
                newName: "IsConfirmed");
        }
    }
}
