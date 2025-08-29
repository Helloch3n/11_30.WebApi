using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _11_30.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class questionbankaddproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnGoing",
                table: "QuestionBank",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnGoing",
                table: "QuestionBank");
        }
    }
}
