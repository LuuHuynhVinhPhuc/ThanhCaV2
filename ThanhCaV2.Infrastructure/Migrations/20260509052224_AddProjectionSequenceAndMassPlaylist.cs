using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhCaV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectionSequenceAndMassPlaylist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectionSequence",
                table: "hymns",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MassPlaylist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HymnId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MassPlaylist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MassPlaylist_hymns_HymnId",
                        column: x => x.HymnId,
                        principalTable: "hymns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MassPlaylist_HymnId",
                table: "MassPlaylist",
                column: "HymnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MassPlaylist");

            migrationBuilder.DropColumn(
                name: "ProjectionSequence",
                table: "hymns");
        }
    }
}
