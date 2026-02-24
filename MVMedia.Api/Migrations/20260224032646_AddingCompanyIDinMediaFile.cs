using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVMedia.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingCompanyIDinMediaFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "MediaFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_CompanyId",
                table: "MediaFiles",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Companies_CompanyId",
                table: "MediaFiles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Companies_CompanyId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_CompanyId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "MediaFiles");
        }
    }
}
