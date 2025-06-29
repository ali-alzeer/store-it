#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class TryAddConstraintGenerateGuidString : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "FileTypes",
          type: "nvarchar(450)",
          nullable: false,
          defaultValueSql: "CAST(NEWID() AS VARCHAR(256))",
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "FileTypes",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)",
          oldDefaultValueSql: "CAST(NEWID() AS VARCHAR(256))");
    }
  }
}
