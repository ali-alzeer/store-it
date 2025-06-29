using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class AddUserFileDtoAndMakeItHasNoKey : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "UserFileDto",
          schema: "dbo",
          columns: table => new
          {
            FileId = table.Column<string>(type: "varchar(36)", nullable: false),
            FileName = table.Column<string>(type: "nvarchar(256)", nullable: false),
            Url = table.Column<string>(type: "nvarchar(256)", nullable: false),
            TypeId = table.Column<int>(type: "varchar(36)", nullable: false),
            FileTypeName = table.Column<string>(type: "nvarchar(50)", nullable: false),
            ExtensionId = table.Column<int>(type: "varchar(36)", nullable: false),
            FileExtensionName = table.Column<string>(type: "nvarchar(50)", nullable: false),
            Size = table.Column<long>(type: "int", nullable: false),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
          },
          constraints: table =>
          {
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "UserFileDto",
          schema: "dbo");
    }
  }
}
