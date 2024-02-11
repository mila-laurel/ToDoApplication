using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoListLibrary.Migrations;

public partial class CustomFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CustomFields",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(nullable: true),
                Value = table.Column<string>(nullable: true),
                ToDoEntryId = table.Column<int>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CustomFields", x => x.Id);
                table.ForeignKey(
                    name: "FK_CustomFields_Entities_ToDoEntryId",
                    column: x => x.ToDoEntryId,
                    principalTable: "Entities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CustomFields_ToDoEntryId",
            table: "CustomFields",
            column: "ToDoEntryId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CustomFields");
    }
}
