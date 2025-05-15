using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssessmentProject.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Discount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Concerts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredTicketsForDiscount",
                table: "Concerts",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "RequiredTicketsForDiscount",
                table: "Concerts");
        }
    }
}
