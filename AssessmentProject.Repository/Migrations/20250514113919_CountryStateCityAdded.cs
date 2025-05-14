using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssessmentProject.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CountryStateCityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ContactNumber",
                table: "Users",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Users");
        }
    }
}
