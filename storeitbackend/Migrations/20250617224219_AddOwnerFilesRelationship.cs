using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class AddOwnerFilesRelationship : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "TypeId",
          schema: "dbo",
          table: "UserFileDto",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<int>(
          name: "Size",
          schema: "dbo",
          table: "UserFileDto",
          type: "int",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "int");

      migrationBuilder.AlterColumn<string>(
          name: "ExtensionId",
          schema: "dbo",
          table: "UserFileDto",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AddColumn<string>(
          name: "OwnerFirstName",
          schema: "dbo",
          table: "UserFileDto",
          type: "nvarchar(15)",
          nullable: false,
          defaultValue: "");

      migrationBuilder.AddColumn<string>(
          name: "OwnerId",
          schema: "dbo",
          table: "UserFileDto",
          type: "varchar(36)",
          nullable: false,
          defaultValue: "");

      migrationBuilder.AddColumn<string>(
          name: "OwnerImageUrl",
          schema: "dbo",
          table: "UserFileDto",
          type: "nvarchar(256)",
          nullable: false,
          defaultValue: "");

      migrationBuilder.AddColumn<string>(
          name: "OwnerLastName",
          schema: "dbo",
          table: "UserFileDto",
          type: "nvarchar(15)",
          nullable: false,
          defaultValue: "");

      migrationBuilder.CreateTable(
          name: "OwnersFiles",
          schema: "dbo",
          columns: table => new
          {
            UserId = table.Column<string>(type: "varchar(36)", nullable: false),
            FileId = table.Column<string>(type: "varchar(36)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_OwnersFiles", x => new { x.UserId, x.FileId });
            table.ForeignKey(
                      name: "FK_OwnersFiles_Files_FileId",
                      column: x => x.FileId,
                      principalSchema: "files",
                      principalTable: "Files",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_OwnersFiles_Users_UserId",
                      column: x => x.UserId,
                      principalSchema: "identity",
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_OwnersFiles_FileId",
          schema: "dbo",
          table: "OwnersFiles",
          column: "FileId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "OwnersFiles",
          schema: "dbo");

      migrationBuilder.DropColumn(
          name: "OwnerFirstName",
          schema: "dbo",
          table: "UserFileDto");

      migrationBuilder.DropColumn(
          name: "OwnerId",
          schema: "dbo",
          table: "UserFileDto");

      migrationBuilder.DropColumn(
          name: "OwnerImageUrl",
          schema: "dbo",
          table: "UserFileDto");

      migrationBuilder.DropColumn(
          name: "OwnerLastName",
          schema: "dbo",
          table: "UserFileDto");

      migrationBuilder.AlterColumn<string>(
          name: "TypeId",
          schema: "dbo",
          table: "UserFileDto",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<int>(
          name: "Size",
          schema: "dbo",
          table: "UserFileDto",
          type: "int",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "int");

      migrationBuilder.AlterColumn<string>(
          name: "ExtensionId",
          schema: "dbo",
          table: "UserFileDto",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");
    }
  }
}
