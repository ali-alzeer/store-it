#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class RemappedUserNameBecauseOfError : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "NormalizedUserName",
          schema: "identity",
          table: "Users",
          type: "nvarchar(256)",
          maxLength: 256,
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "UserName",
          schema: "identity",
          table: "Users",
          type: "nvarchar(256)",
          maxLength: 256,
          nullable: true);

      migrationBuilder.CreateIndex(
          name: "UserNameIndex",
          schema: "identity",
          table: "Users",
          column: "NormalizedUserName",
          unique: true,
          filter: "[NormalizedUserName] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "UserNameIndex",
          schema: "identity",
          table: "Users");

      migrationBuilder.DropColumn(
          name: "NormalizedUserName",
          schema: "identity",
          table: "Users");

      migrationBuilder.DropColumn(
          name: "UserName",
          schema: "identity",
          table: "Users");
    }
  }
}
