using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AHDownloader.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuctionSnaps",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionSnaps", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BonusLists",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BonusListId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusLists", x => x.ID);
                    table.UniqueConstraint("AK_BonusLists_BonusListId", x => x.BonusListId);
                });

            migrationBuilder.CreateTable(
                name: "Modifiers",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<long>(nullable: false),
                    Value = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifiers", x => x.ID);
                    table.UniqueConstraint("AK_Modifiers_Type_Value", x => new { x.Type, x.Value });
                });

            migrationBuilder.CreateTable(
                name: "RealmDatas",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Realm = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealmDatas", x => x.ID);
                    table.UniqueConstraint("AK_RealmDatas_Realm", x => x.Realm);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuctionID = table.Column<int>(nullable: false),
                    Item = table.Column<long>(nullable: false),
                    Owner = table.Column<string>(nullable: true),
                    OwnerRealm = table.Column<string>(nullable: true),
                    Bid = table.Column<long>(nullable: false),
                    Buyout = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    TimeLeft = table.Column<int>(nullable: false),
                    PetSpeciesId = table.Column<long>(nullable: true),
                    PetBreedId = table.Column<long>(nullable: true),
                    PetLevel = table.Column<long>(nullable: true),
                    PetQualityId = table.Column<long>(nullable: true),
                    AuctionSnapID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Auctions_AuctionSnaps_AuctionSnapID",
                        column: x => x.AuctionSnapID,
                        principalTable: "AuctionSnaps",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionSnapRealmDatas",
                columns: table => new
                {
                    AuctionSnapID = table.Column<long>(nullable: false),
                    RealmDataID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionSnapRealmDatas", x => new { x.AuctionSnapID, x.RealmDataID });
                    table.ForeignKey(
                        name: "FK_AuctionSnapRealmDatas_AuctionSnaps_AuctionSnapID",
                        column: x => x.AuctionSnapID,
                        principalTable: "AuctionSnaps",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionSnapRealmDatas_RealmDatas_RealmDataID",
                        column: x => x.RealmDataID,
                        principalTable: "RealmDatas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionBonusLists",
                columns: table => new
                {
                    AuctionID = table.Column<long>(nullable: false),
                    BonusListID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionBonusLists", x => new { x.AuctionID, x.BonusListID });
                    table.ForeignKey(
                        name: "FK_AuctionBonusLists_Auctions_AuctionID",
                        column: x => x.AuctionID,
                        principalTable: "Auctions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionBonusLists_BonusLists_BonusListID",
                        column: x => x.BonusListID,
                        principalTable: "BonusLists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionModifiers",
                columns: table => new
                {
                    AuctionID = table.Column<long>(nullable: false),
                    ModifierID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionModifiers", x => new { x.AuctionID, x.ModifierID });
                    table.ForeignKey(
                        name: "FK_AuctionModifiers_Auctions_AuctionID",
                        column: x => x.AuctionID,
                        principalTable: "Auctions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionModifiers_Modifiers_ModifierID",
                        column: x => x.ModifierID,
                        principalTable: "Modifiers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBonusLists_BonusListID",
                table: "AuctionBonusLists",
                column: "BonusListID");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionModifiers_ModifierID",
                table: "AuctionModifiers",
                column: "ModifierID");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_AuctionSnapID",
                table: "Auctions",
                column: "AuctionSnapID");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionSnapRealmDatas_RealmDataID",
                table: "AuctionSnapRealmDatas",
                column: "RealmDataID");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionSnaps_TimeStamp",
                table: "AuctionSnaps",
                column: "TimeStamp",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionBonusLists");

            migrationBuilder.DropTable(
                name: "AuctionModifiers");

            migrationBuilder.DropTable(
                name: "AuctionSnapRealmDatas");

            migrationBuilder.DropTable(
                name: "BonusLists");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "Modifiers");

            migrationBuilder.DropTable(
                name: "RealmDatas");

            migrationBuilder.DropTable(
                name: "AuctionSnaps");
        }
    }
}
