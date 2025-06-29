using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class MakeUserFileDtoImageUrlNullable : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "OwnerImageUrl",
          schema: "dbo",
          table: "UserFileDto",
          type: "nvarchar(256)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(256)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "OwnerImageUrl",
          schema: "dbo",
          table: "UserFileDto",
          type: "nvarchar(256)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(256)",
          oldNullable: true);
    }
  }
}
