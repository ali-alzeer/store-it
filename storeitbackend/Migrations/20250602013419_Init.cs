#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class Init : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.EnsureSchema(
          name: "files");

      migrationBuilder.EnsureSchema(
          name: "identity");

      migrationBuilder.EnsureSchema(
          name: "dbo");

      migrationBuilder.CreateTable(
          name: "FileExtensions",
          schema: "files",
          columns: table => new
          {
            Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FileExtensions", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "FileTypes",
          schema: "files",
          columns: table => new
          {
            Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FileTypes", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Roles",
          schema: "identity",
          columns: table => new
          {
            Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            NormalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Roles", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Users",
          schema: "identity",
          columns: table => new
          {
            Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            FirstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            LastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            NormalizedEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
            TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Users", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Files",
          schema: "files",
          columns: table => new
          {
            Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            TypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            ExtensionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Size = table.Column<int>(type: "int", nullable: false),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Files", x => x.Id);
            table.ForeignKey(
                      name: "FK_Files_FileExtensions_ExtensionId",
                      column: x => x.ExtensionId,
                      principalSchema: "files",
                      principalTable: "FileExtensions",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Files_FileTypes_TypeId",
                      column: x => x.TypeId,
                      principalSchema: "files",
                      principalTable: "FileTypes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "RolesClaims",
          schema: "identity",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            ClaimValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_RolesClaims", x => x.Id);
            table.ForeignKey(
                      name: "FK_RolesClaims_Roles_RoleId",
                      column: x => x.RoleId,
                      principalSchema: "identity",
                      principalTable: "Roles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "UsersClaims",
          schema: "identity",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            ClaimValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UsersClaims", x => x.Id);
            table.ForeignKey(
                      name: "FK_UsersClaims_Users_UserId",
                      column: x => x.UserId,
                      principalSchema: "identity",
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "UsersLogins",
          schema: "identity",
          columns: table => new
          {
            LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
            ProviderDisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UsersLogins", x => new { x.LoginProvider, x.ProviderKey });
            table.ForeignKey(
                      name: "FK_UsersLogins_Users_UserId",
                      column: x => x.UserId,
                      principalSchema: "identity",
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "UsersRoles",
          schema: "identity",
          columns: table => new
          {
            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UsersRoles", x => new { x.UserId, x.RoleId });
            table.ForeignKey(
                      name: "FK_UsersRoles_Roles_RoleId",
                      column: x => x.RoleId,
                      principalSchema: "identity",
                      principalTable: "Roles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_UsersRoles_Users_UserId",
                      column: x => x.UserId,
                      principalSchema: "identity",
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "UsersTokens",
          schema: "identity",
          columns: table => new
          {
            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UsersTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            table.ForeignKey(
                      name: "FK_UsersTokens_Users_UserId",
                      column: x => x.UserId,
                      principalSchema: "identity",
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "UsersFiles",
          schema: "dbo",
          columns: table => new
          {
            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            FileId = table.Column<string>(type: "nvarchar(450)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UsersFiles", x => new { x.UserId, x.FileId });
            table.ForeignKey(
                      name: "FK_UsersFiles_Files_FileId",
                      column: x => x.FileId,
                      principalSchema: "files",
                      principalTable: "Files",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_UsersFiles_Users_UserId",
                      column: x => x.UserId,
                      principalSchema: "identity",
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Files_ExtensionId",
          schema: "files",
          table: "Files",
          column: "ExtensionId");

      migrationBuilder.CreateIndex(
          name: "IX_Files_TypeId",
          schema: "files",
          table: "Files",
          column: "TypeId");

      migrationBuilder.CreateIndex(
          name: "RoleNameIndex",
          schema: "identity",
          table: "Roles",
          column: "NormalizedName",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_RolesClaims_RoleId",
          schema: "identity",
          table: "RolesClaims",
          column: "RoleId");

      migrationBuilder.CreateIndex(
          name: "EmailIndex",
          schema: "identity",
          table: "Users",
          column: "NormalizedEmail");

      migrationBuilder.CreateIndex(
          name: "IX_UsersClaims_UserId",
          schema: "identity",
          table: "UsersClaims",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_UsersFiles_FileId",
          schema: "dbo",
          table: "UsersFiles",
          column: "FileId");

      migrationBuilder.CreateIndex(
          name: "IX_UsersLogins_UserId",
          schema: "identity",
          table: "UsersLogins",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_UsersRoles_RoleId",
          schema: "identity",
          table: "UsersRoles",
          column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "RolesClaims",
          schema: "identity");

      migrationBuilder.DropTable(
          name: "UsersClaims",
          schema: "identity");

      migrationBuilder.DropTable(
          name: "UsersFiles",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "UsersLogins",
          schema: "identity");

      migrationBuilder.DropTable(
          name: "UsersRoles",
          schema: "identity");

      migrationBuilder.DropTable(
          name: "UsersTokens",
          schema: "identity");

      migrationBuilder.DropTable(
          name: "Files",
          schema: "files");

      migrationBuilder.DropTable(
          name: "Roles",
          schema: "identity");

      migrationBuilder.DropTable(
          name: "Users",
          schema: "identity");

      migrationBuilder.DropTable(
          name: "FileExtensions",
          schema: "files");

      migrationBuilder.DropTable(
          name: "FileTypes",
          schema: "files");
    }
  }
}
