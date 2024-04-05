using EventManagementApp.Services;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("User", columns: table=> new {
                Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", defaultValueSql: "NEWID()"),
                GivenName = table.Column<string>(type:"NVARCHAR(50)", nullable:false),
                MiddleName = table.Column<string>(type:"NVARCHAR(50)", nullable:false),
                Surname = table.Column<string>(type:"NVARCHAR(50)", nullable:false),
                CreatedAt = table.Column<DateTime>(type: "DATETIME", defaultValueSql: "GETDATE()"),
                DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                LoginCredentialId = table.Column<Guid>(type: "UNIQUEIDENTIFIER")
            }, constraints : table => {
                table.PrimaryKey("PK_User", row=> row.Id);
                table.ForeignKey(name: "FK_User_LoginCredential_LoginCredentialId", column: row=> row.LoginCredentialId, 
                principalTable:"LoginCredential", principalColumn: "Id");
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("User");
        }
    }
}
