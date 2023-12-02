using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyMaker.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompleteToResponseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Responses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Responses");
        }
    }
}
