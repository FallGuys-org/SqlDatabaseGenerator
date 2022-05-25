using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FGO.Database.Generator.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalisedString",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    English = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalisedString", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    NameId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currencies_LocalisedString_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomisationItemTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    NameId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomisationItemTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomisationItemTypes_LocalisedString_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rarities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    NameId = table.Column<int>(type: "INTEGER", nullable: true),
                    BackgroundColor = table.Column<int>(type: "INTEGER", nullable: false),
                    ForegroundColor = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rarities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rarities_LocalisedString_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    NameId = table.Column<int>(type: "INTEGER", nullable: true),
                    ThemeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    End = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_LocalisedString_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seasons_LocalisedString_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomisationItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    NameId = table.Column<int>(type: "INTEGER", nullable: true),
                    RarityId = table.Column<string>(type: "TEXT", nullable: true),
                    ItemTypeId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomisationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomisationItems_CustomisationItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "CustomisationItemTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomisationItems_LocalisedString_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomisationItems_Rarities_RarityId",
                        column: x => x.RarityId,
                        principalTable: "Rarities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomisationItemSources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    NameId = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomisationItemId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomisationItemSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomisationItemSources_CustomisationItems_CustomisationItemId",
                        column: x => x.CustomisationItemId,
                        principalTable: "CustomisationItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomisationItemSources_LocalisedString_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemPrice",
                columns: table => new
                {
                    CustomisationItemId = table.Column<string>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    CurrencyId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPrice", x => new { x.CustomisationItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ItemPrice_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemPrice_CustomisationItems_CustomisationItemId",
                        column: x => x.CustomisationItemId,
                        principalTable: "CustomisationItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LabelId = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomisationItemId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_CustomisationItems_CustomisationItemId",
                        column: x => x.CustomisationItemId,
                        principalTable: "CustomisationItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tag_LocalisedString_LabelId",
                        column: x => x.LabelId,
                        principalTable: "LocalisedString",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_NameId",
                table: "Currencies",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomisationItems_ItemTypeId",
                table: "CustomisationItems",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomisationItems_NameId",
                table: "CustomisationItems",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomisationItems_RarityId",
                table: "CustomisationItems",
                column: "RarityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomisationItemSources_CustomisationItemId",
                table: "CustomisationItemSources",
                column: "CustomisationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomisationItemSources_NameId",
                table: "CustomisationItemSources",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomisationItemTypes_NameId",
                table: "CustomisationItemTypes",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrice_CurrencyId",
                table: "ItemPrice",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Rarities_NameId",
                table: "Rarities",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_NameId",
                table: "Seasons",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_ThemeId",
                table: "Seasons",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CustomisationItemId",
                table: "Tag",
                column: "CustomisationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LabelId",
                table: "Tag",
                column: "LabelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomisationItemSources");

            migrationBuilder.DropTable(
                name: "ItemPrice");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "CustomisationItems");

            migrationBuilder.DropTable(
                name: "CustomisationItemTypes");

            migrationBuilder.DropTable(
                name: "Rarities");

            migrationBuilder.DropTable(
                name: "LocalisedString");
        }
    }
}
