using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesAnalytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfSales",
                table: "SaleRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfSales",
                table: "SaleRecords");
        }
    }
}
