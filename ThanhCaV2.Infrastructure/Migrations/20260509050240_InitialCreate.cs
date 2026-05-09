using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhCaV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hymns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    author = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    season = table.Column<string>(type: "TEXT", nullable: false),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    last_modified_by = table.Column<string>(type: "TEXT", nullable: true),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hymns", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lyric_sections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    hymn_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    order = table.Column<int>(type: "INTEGER", nullable: false),
                    type = table.Column<string>(type: "TEXT", nullable: false),
                    label = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lyric_sections", x => x.id);
                    table.ForeignKey(
                        name: "FK_lyric_sections_hymns_hymn_id",
                        column: x => x.hymn_id,
                        principalTable: "hymns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lyric_sections_hymn_id",
                table: "lyric_sections",
                column: "hymn_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lyric_sections");

            migrationBuilder.DropTable(
                name: "hymns");
        }
    }
}
