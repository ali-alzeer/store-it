using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class AddUsersForFileSharedDto : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "UserFileSharedDto",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true),
            FirstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            LastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
          name: "UserFileSharedDto",
          schema: "dbo");
    }
  }
}
