#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class IgnoredPhoneNumberConfirmed : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "PhoneNumberConfirmed",
          schema: "identity",
          table: "Users");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
          name: "PhoneNumberConfirmed",
          schema: "identity",
          table: "Users",
          type: "bit",
          nullable: false,
          defaultValue: false);
    }
  }
}
