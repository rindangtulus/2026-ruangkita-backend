using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_ruangkita_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BorrowingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StatusHistoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusHistories_Borrowings_BorrowingId",
                        column: x => x.BorrowingId,
                        principalTable: "Borrowings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusHistories_StatusHistories_StatusHistoryId",
                        column: x => x.StatusHistoryId,
                        principalTable: "StatusHistories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistories_BorrowingId",
                table: "StatusHistories",
                column: "BorrowingId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistories_StatusHistoryId",
                table: "StatusHistories",
                column: "StatusHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusHistories");
        }
    }
}
