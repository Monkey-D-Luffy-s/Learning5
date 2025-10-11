using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning5.Migrations
{
    /// <inheritdoc />
    public partial class AddingFlaginBanks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flag",
                table: "EmployeeTaxDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flag",
                table: "EmployeeStaturary",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flag",
                table: "EmployeeSalaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flag",
                table: "BankDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag",
                table: "EmployeeTaxDetails");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "EmployeeStaturary");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "EmployeeSalaries");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "BankDetails");
        }
    }
}
