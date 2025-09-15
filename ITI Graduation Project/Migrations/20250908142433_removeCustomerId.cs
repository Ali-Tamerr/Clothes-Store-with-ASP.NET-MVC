using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class removeCustomerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
