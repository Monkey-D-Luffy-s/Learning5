using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning5.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Colleges_UserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Colleges");

            migrationBuilder.AddColumn<string>(
                name: "CollegeCode",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CollegeCode",
                table: "AspNetUsers",
                column: "CollegeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Colleges_CollegeCode",
                table: "AspNetUsers",
                column: "CollegeCode",
                principalTable: "Colleges",
                principalColumn: "CollegeCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Colleges_CollegeCode",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CollegeCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CollegeCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Colleges_UserName",
                table: "AspNetUsers",
                column: "UserName",
                principalTable: "Colleges",
                principalColumn: "CollegeCode");
        }
    }
}
