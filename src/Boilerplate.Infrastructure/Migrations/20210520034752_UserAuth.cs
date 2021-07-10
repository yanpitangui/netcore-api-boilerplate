using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Boilerplate.Infrastructure.Migrations
{
    public partial class UserAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[] { new Guid("687d9fd5-2752-4a96-93d5-0f33a49913c6"), "admin@boilerplate.com", "$2a$11$oNUUOd3qRUAGD55pG/VBR.p3kn4kgdk5qJ6ArL5UybYMI0kFOlX4i", "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[] { new Guid("6648c89f-e894-42bb-94f0-8fd1059c86b4"), "user@boilerplate.com", "$2a$11$COfh6eYz/zaenoTtBexF7ueQmxbUo5PJJPdyR/HYoqDmolhWpZ3ui", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
