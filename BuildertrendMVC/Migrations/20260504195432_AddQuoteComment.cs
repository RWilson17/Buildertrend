using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildertrendMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddQuoteComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuoteAttachment_Quotes_QuoteId",
                table: "QuoteAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteAttachment",
                table: "QuoteAttachment");

            migrationBuilder.RenameTable(
                name: "QuoteAttachment",
                newName: "QuoteAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_QuoteAttachment_QuoteId",
                table: "QuoteAttachments",
                newName: "IX_QuoteAttachments_QuoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteAttachments",
                table: "QuoteAttachments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QuoteComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteComments_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteComments_QuoteId",
                table: "QuoteComments",
                column: "QuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteAttachments_Quotes_QuoteId",
                table: "QuoteAttachments",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuoteAttachments_Quotes_QuoteId",
                table: "QuoteAttachments");

            migrationBuilder.DropTable(
                name: "QuoteComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteAttachments",
                table: "QuoteAttachments");

            migrationBuilder.RenameTable(
                name: "QuoteAttachments",
                newName: "QuoteAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_QuoteAttachments_QuoteId",
                table: "QuoteAttachment",
                newName: "IX_QuoteAttachment_QuoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteAttachment",
                table: "QuoteAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteAttachment_Quotes_QuoteId",
                table: "QuoteAttachment",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
