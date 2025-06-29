#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeitbackend.Migrations
{
  /// <inheritdoc />
  public partial class AddedIdGuidStringsInAppDbContext : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        name: "FK_UsersTokens_Users_UserId",
        schema: "identity",
        table: "UsersTokens");

      migrationBuilder.DropPrimaryKey(
          name: "PK_UsersTokens",
          schema: "identity",
          table: "UsersTokens");

      migrationBuilder.DropForeignKey(
        name: "FK_UsersRoles_Users_UserId",
        schema: "identity",
        table: "UsersRoles");


      migrationBuilder.DropForeignKey(
        name: "FK_UsersRoles_Roles_RoleId",
        schema: "identity",
        table: "UsersRoles");


      migrationBuilder.DropPrimaryKey(
          name: "PK_UsersRoles",
          schema: "identity",
          table: "UsersRoles");


      migrationBuilder.DropForeignKey(
        name: "FK_UsersLogins_Users_UserId",
        schema: "identity",
        table: "UsersLogins");

      migrationBuilder.DropForeignKey(
        name: "FK_UsersFiles_Users_UserId",
        schema: "dbo",
        table: "UsersFiles");


      migrationBuilder.DropForeignKey(
        name: "FK_UsersFiles_Files_FileId",
        schema: "dbo",
        table: "UsersFiles");


      migrationBuilder.DropPrimaryKey(
          name: "PK_UsersFiles",
          schema: "dbo",
          table: "UsersFiles");

      migrationBuilder.DropForeignKey(
        name: "FK_UsersClaims_Users_UserId",
        schema: "identity",
        table: "UsersClaims");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Users",
          schema: "identity",
          table: "Users");

      migrationBuilder.DropForeignKey(
        name: "FK_RolesClaims_Roles_RoleId",
        schema: "identity",
        table: "RolesClaims");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Roles",
          schema: "identity",
          table: "Roles");


      migrationBuilder.DropForeignKey(
        name: "FK_Files_FileTypes_TypeId",
        schema: "files",
        table: "Files");

      migrationBuilder.DropForeignKey(
        name: "FK_Files_FileExtensions_ExtensionId",
        schema: "files",
        table: "Files");


      migrationBuilder.DropPrimaryKey(
          name: "PK_FileTypes",
          schema: "files",
          table: "FileTypes");

      migrationBuilder.DropPrimaryKey(
          name: "PK_FileExtensions",
          schema: "files",
          table: "FileExtensions");



      migrationBuilder.DropPrimaryKey(
          name: "PK_Files",
          schema: "files",
          table: "Files");






      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersTokens",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "RoleId",
          schema: "identity",
          table: "UsersRoles",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersRoles",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersLogins",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "FileId",
          schema: "dbo",
          table: "UsersFiles",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "dbo",
          table: "UsersFiles",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersClaims",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "identity",
          table: "Users",
          type: "varchar(36)",
          maxLength: 36,
          nullable: false,
          defaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))",
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "RoleId",
          schema: "identity",
          table: "RolesClaims",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "identity",
          table: "Roles",
          type: "varchar(36)",
          maxLength: 36,
          nullable: false,
          defaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))",
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "FileTypes",
          type: "varchar(36)",
          maxLength: 36,
          nullable: false,
          defaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))",
          oldClrType: typeof(string),
          oldType: "nvarchar(450)",
          oldDefaultValueSql: "CAST(NEWID() AS VARCHAR(256))");

      migrationBuilder.AlterColumn<string>(
          name: "TypeId",
          schema: "files",
          table: "Files",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "ExtensionId",
          schema: "files",
          table: "Files",
          type: "varchar(36)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "Files",
          type: "varchar(36)",
          maxLength: 36,
          nullable: false,
          defaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))",
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "FileExtensions",
          type: "varchar(36)",
          maxLength: 36,
          nullable: false,
          defaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))",
          oldClrType: typeof(string),
          oldType: "nvarchar(450)");


      migrationBuilder.AddPrimaryKey(
        name: "PK_Roles",
        schema: "identity",
        table: "Roles",
        column: "Id");


      migrationBuilder.AddPrimaryKey(
        name: "PK_FileTypes",
        schema: "files",
        table: "FileTypes",
        column: "Id");

      migrationBuilder.AddPrimaryKey(
        name: "PK_FileExtensions",
        schema: "files",
        table: "FileExtensions",
        column: "Id");


      migrationBuilder.AddPrimaryKey(
        name: "PK_Users",
        schema: "identity",
        table: "Users",
        column: "Id");


      migrationBuilder.AddPrimaryKey(
        name: "PK_Files",
        schema: "files",
        table: "Files",
        column: "Id");

      migrationBuilder.AddForeignKey(
          name: "FK_UsersTokens_Users_UserId",
          schema: "identity",
          table: "UsersTokens",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddPrimaryKey(
          name: "PK_UsersTokens",
          schema: "identity",
          table: "UsersTokens",
          columns: new[] { "UserId", "LoginProvider", "Name" });


      migrationBuilder.AddPrimaryKey(
          name: "PK_UsersRoles",
          schema: "identity",
          table: "UsersRoles",
          columns: new[] { "UserId", "RoleId" });

      migrationBuilder.AddForeignKey(
          name: "FK_UsersRoles_Roles_RoleId",
          schema: "identity",
          table: "UsersRoles",
          column: "RoleId",
          principalSchema: "identity",
          principalTable: "Roles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_UsersRoles_Users_UserId",
          schema: "identity",
          table: "UsersRoles",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
               name: "FK_UsersLogins_Users_UserId",
               schema: "identity",
               table: "UsersLogins",
               column: "UserId",
               principalSchema: "identity",
               principalTable: "Users",
               principalColumn: "Id",
               onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddPrimaryKey(
          name: "PK_UsersFiles",
          schema: "dbo",
          table: "UsersFiles",
          columns: new[] { "UserId", "FileId" });

      migrationBuilder.AddForeignKey(
          name: "FK_UsersFiles_Files_FileId",
          schema: "dbo",
          table: "UsersFiles",
          column: "FileId",
          principalSchema: "files",
          principalTable: "Files",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_UsersFiles_Users_UserId",
          schema: "dbo",
          table: "UsersFiles",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);


      migrationBuilder.AddForeignKey(
          name: "FK_UsersClaims_Users_UserId",
          schema: "identity",
          table: "UsersClaims",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);



      migrationBuilder.AddForeignKey(
          name: "FK_RolesClaims_Roles_RoleId",
          schema: "identity",
          table: "RolesClaims",
          column: "RoleId",
          principalSchema: "identity",
          principalTable: "Roles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);


      migrationBuilder.AddForeignKey(
          name: "FK_Files_FileTypes_TypeId",
          schema: "files",
          table: "Files",
          column: "TypeId",
          principalSchema: "files",
          principalTable: "FileTypes",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_Files_FileExtensions_ExtensionId",
          schema: "files",
          table: "Files",
          column: "ExtensionId",
          principalSchema: "files",
          principalTable: "FileExtensions",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);


    }


    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        name: "FK_UsersTokens_Users_UserId",
        schema: "identity",
        table: "UsersTokens");

      migrationBuilder.DropPrimaryKey(
          name: "PK_UsersTokens",
          schema: "identity",
          table: "UsersTokens");


      migrationBuilder.DropForeignKey(
        name: "FK_UsersRoles_Users_UserId",
        schema: "identity",
        table: "UsersRoles");


      migrationBuilder.DropForeignKey(
        name: "FK_UsersRoles_Roles_RoleId",
        schema: "identity",
        table: "UsersRoles");


      migrationBuilder.DropPrimaryKey(
          name: "PK_UsersRoles",
          schema: "identity",
          table: "UsersRoles");

      migrationBuilder.DropForeignKey(
        name: "FK_UsersLogins_Users_UserId",
        schema: "identity",
        table: "UsersLogins");

      migrationBuilder.DropForeignKey(
  name: "FK_UsersFiles_Users_UserId",
  schema: "dbo",
  table: "UsersFiles");


      migrationBuilder.DropForeignKey(
        name: "FK_UsersFiles_Files_FileId",
        schema: "dbo",
        table: "UsersFiles");


      migrationBuilder.DropPrimaryKey(
          name: "PK_UsersFiles",
          schema: "dbo",
          table: "UsersFiles");

      migrationBuilder.DropForeignKey(
        name: "FK_UsersClaims_Users_UserId",
        schema: "identity",
        table: "UsersClaims");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Users",
          schema: "identity",
          table: "Users");

      migrationBuilder.DropForeignKey(
        name: "FK_RolesClaims_Roles_RoleId",
        schema: "identity",
        table: "RolesClaims");


      migrationBuilder.DropPrimaryKey(
          name: "PK_Roles",
          schema: "identity",
          table: "Roles");


      migrationBuilder.DropForeignKey(
        name: "FK_Files_FileTypes_TypeId",
        schema: "files",
        table: "Files");


      migrationBuilder.DropForeignKey(
        name: "FK_Files_FileExtensions_ExtensionId",
        schema: "files",
        table: "Files");


      migrationBuilder.DropPrimaryKey(
          name: "PK_FileTypes",
          schema: "files",
          table: "FileTypes");


      migrationBuilder.DropPrimaryKey(
          name: "PK_FileExtensions",
          schema: "files",
          table: "FileExtensions");


      migrationBuilder.DropPrimaryKey(
          name: "PK_Files",
          schema: "files",
          table: "Files");



      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersTokens",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "RoleId",
          schema: "identity",
          table: "UsersRoles",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersRoles",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersLogins",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "FileId",
          schema: "dbo",
          table: "UsersFiles",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "dbo",
          table: "UsersFiles",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "UserId",
          schema: "identity",
          table: "UsersClaims",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "identity",
          table: "Users",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)",
          oldMaxLength: 36,
          oldDefaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))");

      migrationBuilder.AlterColumn<string>(
          name: "RoleId",
          schema: "identity",
          table: "RolesClaims",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "identity",
          table: "Roles",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)",
          oldMaxLength: 36,
          oldDefaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "FileTypes",
          type: "nvarchar(450)",
          nullable: false,
          defaultValueSql: "CAST(NEWID() AS VARCHAR(256))",
          oldClrType: typeof(string),
          oldType: "varchar(36)",
          oldMaxLength: 36,
          oldDefaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))");

      migrationBuilder.AlterColumn<string>(
          name: "TypeId",
          schema: "files",
          table: "Files",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "ExtensionId",
          schema: "files",
          table: "Files",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "Files",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)",
          oldMaxLength: 36,
          oldDefaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))");

      migrationBuilder.AlterColumn<string>(
          name: "Id",
          schema: "files",
          table: "FileExtensions",
          type: "nvarchar(450)",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "varchar(36)",
          oldMaxLength: 36,
          oldDefaultValueSql: "UPPER(CAST(NEWID() AS VARCHAR(36)))");


      migrationBuilder.AddPrimaryKey(
        name: "PK_Roles",
        schema: "identity",
        table: "Roles",
        column: "Id");

      migrationBuilder.AddPrimaryKey(
        name: "PK_FileTypes",
        schema: "files",
        table: "FileTypes",
        column: "Id");


      migrationBuilder.AddPrimaryKey(
        name: "PK_FileExtensions",
        schema: "files",
        table: "FileExtensions",
        column: "Id");


      migrationBuilder.AddPrimaryKey(
        name: "PK_Users",
        schema: "identity",
        table: "Users",
        column: "Id");


      migrationBuilder.AddPrimaryKey(
        name: "PK_Files",
        schema: "files",
        table: "Files",
        column: "Id");

      migrationBuilder.AddForeignKey(
          name: "FK_UsersTokens_Users_UserId",
          schema: "identity",
          table: "UsersTokens",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddPrimaryKey(
        name: "PK_UsersTokens",
        schema: "identity",
        table: "UsersTokens",
        columns: new[] { "UserId", "LoginProvider", "Name" });


      migrationBuilder.AddPrimaryKey(
            name: "PK_UsersRoles",
            schema: "identity",
            table: "UsersRoles",
            columns: new[] { "UserId", "RoleId" });

      migrationBuilder.AddForeignKey(
          name: "FK_UsersRoles_Roles_RoleId",
          schema: "identity",
          table: "UsersRoles",
          column: "RoleId",
          principalSchema: "identity",
          principalTable: "Roles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_UsersRoles_Users_UserId",
          schema: "identity",
          table: "UsersRoles",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);


      migrationBuilder.AddPrimaryKey(
          name: "PK_UsersRoles",
          schema: "identity",
          table: "UsersRoles",
          columns: new[] { "UserId", "RoleId" });

      migrationBuilder.AddForeignKey(
          name: "FK_UsersRoles_Roles_RoleId",
          schema: "identity",
          table: "UsersRoles",
          column: "RoleId",
          principalSchema: "identity",
          principalTable: "Roles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_UsersRoles_Users_UserId",
          schema: "identity",
          table: "UsersRoles",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
        name: "FK_UsersLogins_Users_UserId",
        schema: "identity",
        table: "UsersLogins",
        column: "UserId",
        principalSchema: "identity",
        principalTable: "Users",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddPrimaryKey(
          name: "PK_UsersFiles",
          schema: "dbo",
          table: "UsersFiles",
          columns: new[] { "UserId", "FileId" });

      migrationBuilder.AddForeignKey(
          name: "FK_UsersFiles_Files_FileId",
          schema: "dbo",
          table: "UsersFiles",
          column: "FileId",
          principalSchema: "files",
          principalTable: "Files",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_UsersFiles_Users_UserId",
          schema: "dbo",
          table: "UsersFiles",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);


      migrationBuilder.AddForeignKey(
          name: "FK_UsersClaims_Users_UserId",
          schema: "identity",
          table: "UsersClaims",
          column: "UserId",
          principalSchema: "identity",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);


      migrationBuilder.AddForeignKey(
          name: "FK_RolesClaims_Roles_RoleId",
          schema: "identity",
          table: "RolesClaims",
          column: "RoleId",
          principalSchema: "identity",
          principalTable: "Roles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_Files_FileTypes_TypeId",
          schema: "files",
          table: "Files",
          column: "TypeId",
          principalSchema: "files",
          principalTable: "FileTypes",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_Files_FileExtensions_ExtensionId",
          schema: "files",
          table: "Files",
          column: "ExtensionId",
          principalSchema: "files",
          principalTable: "FileExtensions",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

    }
  }
}
