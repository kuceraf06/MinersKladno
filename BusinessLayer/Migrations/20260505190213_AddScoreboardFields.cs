using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miners.Web.BusinessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreboardFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "active",
                schema: "sch_miners",
                table: "tbl_scoreboard",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "type",
                schema: "sch_miners",
                table: "tbl_scoreboard",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "guid_image",
                schema: "sch_miners",
                table: "tbl_article",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "fk_tbl_article_guid_image",
                schema: "sch_miners",
                table: "tbl_article",
                column: "guid_image");

            migrationBuilder.AddForeignKey(
                name: "fk_tbl_article_guid_image",
                schema: "sch_miners",
                table: "tbl_article",
                column: "guid_image",
                principalSchema: "sch_miners",
                principalTable: "tbl_file",
                principalColumn: "guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tbl_article_guid_image",
                schema: "sch_miners",
                table: "tbl_article");

            migrationBuilder.DropIndex(
                name: "fk_tbl_article_guid_image",
                schema: "sch_miners",
                table: "tbl_article");

            migrationBuilder.DropColumn(
                name: "active",
                schema: "sch_miners",
                table: "tbl_scoreboard");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "sch_miners",
                table: "tbl_scoreboard");

            migrationBuilder.AlterColumn<Guid>(
                name: "guid_image",
                schema: "sch_miners",
                table: "tbl_article",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
