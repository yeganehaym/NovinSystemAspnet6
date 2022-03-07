using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OtpCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_UserId",
                table: "OtpCodes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpCodes_Users_UserId",
                table: "OtpCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpCodes_Users_UserId",
                table: "OtpCodes");

            migrationBuilder.DropIndex(
                name: "IX_OtpCodes_UserId",
                table: "OtpCodes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OtpCodes");
        }
    }
}
