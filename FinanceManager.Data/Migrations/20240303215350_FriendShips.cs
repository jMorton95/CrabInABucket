using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class FriendShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransaction_Account_RecipientAccountId",
                table: "RecurringTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_RecipientAccountId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_SenderAccountId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_RecipientAccountId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_SenderAccountId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_RecurringTransaction_RecipientAccountId",
                table: "RecurringTransaction");

            migrationBuilder.RenameColumn(
                name: "SenderAccount",
                table: "RecurringTransaction",
                newName: "SenderAccountId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastOnline",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "RecurringTransaction",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EditedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageContent = table.Column<string>(type: "text", nullable: false),
                    FriendshipId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EditedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Friendship_FriendshipId",
                        column: x => x.FriendshipId,
                        principalTable: "Friendship",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_User_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFriendship",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FriendshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EditedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriendship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendship_Users",
                        column: x => x.FriendshipId,
                        principalTable: "Friendship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFriendship_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransaction_AccountId",
                table: "RecurringTransaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FriendshipId",
                table: "Message",
                column: "FriendshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RecipientId",
                table: "Message",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriendship_FriendshipId",
                table: "UserFriendship",
                column: "FriendshipId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriendship_UserId",
                table: "UserFriendship",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransaction_Account_AccountId",
                table: "RecurringTransaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringTransaction_Account_AccountId",
                table: "RecurringTransaction");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "UserFriendship");

            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.DropIndex(
                name: "IX_RecurringTransaction_AccountId",
                table: "RecurringTransaction");

            migrationBuilder.DropColumn(
                name: "LastOnline",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "RecurringTransaction");

            migrationBuilder.RenameColumn(
                name: "SenderAccountId",
                table: "RecurringTransaction",
                newName: "SenderAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_RecipientAccountId",
                table: "Transaction",
                column: "RecipientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SenderAccountId",
                table: "Transaction",
                column: "SenderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransaction_RecipientAccountId",
                table: "RecurringTransaction",
                column: "RecipientAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringTransaction_Account_RecipientAccountId",
                table: "RecurringTransaction",
                column: "RecipientAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_RecipientAccountId",
                table: "Transaction",
                column: "RecipientAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_SenderAccountId",
                table: "Transaction",
                column: "SenderAccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
