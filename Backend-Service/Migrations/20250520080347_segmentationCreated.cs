using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_Service.Migrations
{
    /// <inheritdoc />
    public partial class segmentationCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Labels_LabelId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_LabelId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageIds",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "ImageLabel",
                columns: table => new
                {
                    ImagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    LabelsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLabel", x => new { x.ImagesId, x.LabelsId });
                    table.ForeignKey(
                        name: "FK_ImageLabel_Images_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageLabel_Labels_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Segmentations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstCoordinate = table.Column<double>(type: "double precision", nullable: false),
                    SecondCoordinate = table.Column<double>(type: "double precision", nullable: false),
                    LabelId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segmentations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segmentations_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segmentations_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageLabel_LabelsId",
                table: "ImageLabel",
                column: "LabelsId");

            migrationBuilder.CreateIndex(
                name: "IX_Segmentations_ImageId",
                table: "Segmentations",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Segmentations_LabelId",
                table: "Segmentations",
                column: "LabelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageLabel");

            migrationBuilder.DropTable(
                name: "Segmentations");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "ImageIds",
                table: "Labels",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LabelId",
                table: "Images",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_LabelId",
                table: "Images",
                column: "LabelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Labels_LabelId",
                table: "Images",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id");
        }
    }
}
