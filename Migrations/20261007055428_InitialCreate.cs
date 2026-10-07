using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechShopApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER_FCM",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    FCM_TOKEN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_FCM", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "userId",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userId", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    GOOGLE_ID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VERIFICATION_CODES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EXPIRES_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IS_USED = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    USED_AT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VERIFICATION_CODES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "auth_providers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    provider_user_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    provider_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    access_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token_expires_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_providers", x => x.id);
                    table.ForeignKey(
                        name: "FK_auth_providers_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "userId",
                column: "ID",
                value: 10000);

            migrationBuilder.CreateIndex(
                name: "IX_auth_providers_user_id",
                table: "auth_providers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_providers");

            migrationBuilder.DropTable(
                name: "USER_FCM");

            migrationBuilder.DropTable(
                name: "userId");

            migrationBuilder.DropTable(
                name: "VERIFICATION_CODES");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
