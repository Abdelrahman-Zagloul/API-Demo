using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Demo.Migrations
{
    /// <inheritdoc />
    public partial class addMetaDataTable12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Metadata",
                newName: "FullPath");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Metadata",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Metadata");

            migrationBuilder.RenameColumn(
                name: "FullPath",
                table: "Metadata",
                newName: "Type");
        }
    }
}
