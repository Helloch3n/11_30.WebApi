using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _11_30.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addgeneralQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionBankId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GeneralQueryType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RequestContent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralQueryType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralQueryAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeneralQueryTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralQueryAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralQueryAction_GeneralQueryType_GeneralQueryTypeId",
                        column: x => x.GeneralQueryTypeId,
                        principalTable: "GeneralQueryType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralQueryField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneralQueryActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralQueryField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralQueryField_GeneralQueryAction_GeneralQueryActionId",
                        column: x => x.GeneralQueryActionId,
                        principalTable: "GeneralQueryAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralQueryAction_GeneralQueryTypeId",
                table: "GeneralQueryAction",
                column: "GeneralQueryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralQueryField_GeneralQueryActionId",
                table: "GeneralQueryField",
                column: "GeneralQueryActionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralQueryField");

            migrationBuilder.DropTable(
                name: "GeneralQueryAction");

            migrationBuilder.DropTable(
                name: "GeneralQueryType");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionBankId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
