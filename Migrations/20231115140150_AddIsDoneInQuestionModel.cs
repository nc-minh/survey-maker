using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyMaker.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDoneInQuestionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Questions",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Questions");
        }
    }
}
