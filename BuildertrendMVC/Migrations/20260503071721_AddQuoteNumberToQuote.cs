using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildertrendMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddQuoteNumberToQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuoteNumber",
                table: "Quotes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuoteNumber",
                table: "Quotes");
        }
    }
}
