using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateLoginCredentialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(name:"LoginCredential", columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", defaultValueSql: "NEWID()"),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsRoot = table.Column<bool>(type: "bit", defaultValueSql: "0"),
                CreatedAt = table.Column<DateTime>(type: "datetime", defaultValueSql: "GETDATE()")
            },
            constraints: table =>{
                table.PrimaryKey("PK_LoginCredential", row=>row.Id);
            }
           );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name:"LoginCredential");
        }
    }
}
