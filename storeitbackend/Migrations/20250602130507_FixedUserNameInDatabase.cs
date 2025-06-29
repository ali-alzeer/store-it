#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class FixedUserNameInDatabase : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "UserNameIndex",
          schema: "identity",
          table: "Users");

      migrationBuilder.AlterColumn<string>(
          name: "UserName",
          schema: "identity",
          table: "Users",
          type: "nvarchar(50)",
          maxLength: 50,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(256)",
          oldMaxLength: 256,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "NormalizedUserName",
          schema: "identity",
          table: "Users",
          type: "nvarchar(50)",
          maxLength: 50,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(256)",
          oldMaxLength: 256,
          oldNullable: true);

      migrationBuilder.CreateIndex(
          name: "UserNameIndex",
          schema: "identity",
          table: "Users",
          column: "NormalizedUserName",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "UserNameIndex",
          schema: "identity",
          table: "Users");

      migrationBuilder.AlterColumn<string>(
          name: "UserName",
          schema: "identity",
          table: "Users",
          type: "nvarchar(256)",
          maxLength: 256,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(50)",
          oldMaxLength: 50);

      migrationBuilder.AlterColumn<string>(
          name: "NormalizedUserName",
          schema: "identity",
          table: "Users",
          type: "nvarchar(256)",
          maxLength: 256,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(50)",
          oldMaxLength: 50);

      migrationBuilder.CreateIndex(
          name: "UserNameIndex",
          schema: "identity",
          table: "Users",
          column: "NormalizedUserName",
          unique: true,
          filter: "[NormalizedUserName] IS NOT NULL");
    }
  }
}
