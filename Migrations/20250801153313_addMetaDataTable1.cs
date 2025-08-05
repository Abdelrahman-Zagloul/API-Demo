using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Demo.Migrations
{
    /// <inheritdoc />
    public partial class addMetaDataTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeInMB",
                table: "Metadata");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Metadata",
                newName: "Type");

            migrationBuilder.AddColumn<long>(
                name: "SizeInByte",
                table: "Metadata",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeInByte",
                table: "Metadata");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Metadata",
                newName: "type");

            migrationBuilder.AddColumn<int>(
                name: "SizeInMB",
                table: "Metadata",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
