using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Demo.Migrations
{
    /// <inheritdoc />
    public partial class addPermission1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission");

            migrationBuilder.RenameTable(
                name: "UserPermission",
                newName: "Permissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                columns: new[] { "Id", "ApplicationUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "UserPermission");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission",
                columns: new[] { "Id", "ApplicationUserId" });
        }
    }
}
