using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_Service.Migrations
{
    /// <inheritdoc />
    public partial class nedbejeeeewfwfew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_FileName",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_DataSets_DataSetName",
                table: "DataSets");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FileName",
                table: "Images",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_DataSetName",
                table: "DataSets",
                column: "DataSetName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_FileName",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_DataSets_DataSetName",
                table: "DataSets");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FileName",
                table: "Images",
                column: "FileName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_DataSetName",
                table: "DataSets",
                column: "DataSetName",
                unique: true);
        }
    }
}
