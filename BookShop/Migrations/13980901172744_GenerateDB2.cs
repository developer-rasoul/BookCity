using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShop.Migrations
{
    public partial class GenerateDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_author_Books_Authors_AuthorID",
                table: "author_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_author_Books_BookInfo_BookID",
                table: "author_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_BookInfo_SubCategories_SubCategoryID",
                table: "BookInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_order_Books_BookInfo_BookID",
                table: "order_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_order_Books_Orders_OrderID",
                table: "order_Books");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_Books",
                table: "order_Books");

            migrationBuilder.DropIndex(
                name: "IX_BookInfo_SubCategoryID",
                table: "BookInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_author_Books",
                table: "author_Books");

            migrationBuilder.DropIndex(
                name: "IX_author_Books_BookID",
                table: "author_Books");

            migrationBuilder.DropColumn(
                name: "Author_BookID",
                table: "author_Books");

            migrationBuilder.RenameTable(
                name: "order_Books",
                newName: "Order_Books");

            migrationBuilder.RenameTable(
                name: "author_Books",
                newName: "Author_Books");

            migrationBuilder.RenameIndex(
                name: "IX_order_Books_OrderID",
                table: "Order_Books",
                newName: "IX_Order_Books_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_order_Books_BookID",
                table: "Order_Books",
                newName: "IX_Order_Books_BookID");

            migrationBuilder.RenameColumn(
                name: "SubCategoryID",
                table: "BookInfo",
                newName: "Weight");

            migrationBuilder.RenameIndex(
                name: "IX_author_Books_AuthorID",
                table: "Author_Books",
                newName: "IX_Author_Books_AuthorID");

            migrationBuilder.AddColumn<int>(
                name: "ParentID",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "BookInfo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "BookInfo",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "BookInfo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "BookInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublishYear",
                table: "BookInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PublisherID",
                table: "BookInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_Books",
                table: "Order_Books",
                column: "Order_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Author_Books",
                table: "Author_Books",
                columns: new[] { "BookID", "AuthorID" });

            migrationBuilder.CreateTable(
                name: "Category_Books",
                columns: table => new
                {
                    BookID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category_Books", x => new { x.BookID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_Category_Books_BookInfo_BookID",
                        column: x => x.BookID,
                        principalTable: "BookInfo",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Category_Books_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PublisherName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PublisherID);
                });

            migrationBuilder.CreateTable(
                name: "Translators",
                columns: table => new
                {
                    TranslatorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translators", x => x.TranslatorID);
                });

            migrationBuilder.CreateTable(
                name: "Translator_Books",
                columns: table => new
                {
                    BookID = table.Column<int>(nullable: false),
                    TranslatorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translator_Books", x => new { x.BookID, x.TranslatorID });
                    table.ForeignKey(
                        name: "FK_Translator_Books_BookInfo_BookID",
                        column: x => x.BookID,
                        principalTable: "BookInfo",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Translator_Books_Translators_TranslatorID",
                        column: x => x.TranslatorID,
                        principalTable: "Translators",
                        principalColumn: "TranslatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentID",
                table: "Categories",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_BookInfo_PublisherID",
                table: "BookInfo",
                column: "PublisherID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Books_CategoryID",
                table: "Category_Books",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Translator_Books_TranslatorID",
                table: "Translator_Books",
                column: "TranslatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Books_Authors_AuthorID",
                table: "Author_Books",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Books_BookInfo_BookID",
                table: "Author_Books",
                column: "BookID",
                principalTable: "BookInfo",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookInfo_Publishers_PublisherID",
                table: "BookInfo",
                column: "PublisherID",
                principalTable: "Publishers",
                principalColumn: "PublisherID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentID",
                table: "Categories",
                column: "ParentID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Books_BookInfo_BookID",
                table: "Order_Books",
                column: "BookID",
                principalTable: "BookInfo",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Books_Orders_OrderID",
                table: "Order_Books",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Books_Authors_AuthorID",
                table: "Author_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Author_Books_BookInfo_BookID",
                table: "Author_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_BookInfo_Publishers_PublisherID",
                table: "BookInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentID",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Books_BookInfo_BookID",
                table: "Order_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Books_Orders_OrderID",
                table: "Order_Books");

            migrationBuilder.DropTable(
                name: "Category_Books");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Translator_Books");

            migrationBuilder.DropTable(
                name: "Translators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_Books",
                table: "Order_Books");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentID",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_BookInfo_PublisherID",
                table: "BookInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Author_Books",
                table: "Author_Books");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "BookInfo");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "BookInfo");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "BookInfo");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "BookInfo");

            migrationBuilder.DropColumn(
                name: "PublishYear",
                table: "BookInfo");

            migrationBuilder.DropColumn(
                name: "PublisherID",
                table: "BookInfo");

            migrationBuilder.RenameTable(
                name: "Order_Books",
                newName: "order_Books");

            migrationBuilder.RenameTable(
                name: "Author_Books",
                newName: "author_Books");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Books_OrderID",
                table: "order_Books",
                newName: "IX_order_Books_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Books_BookID",
                table: "order_Books",
                newName: "IX_order_Books_BookID");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "BookInfo",
                newName: "SubCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Author_Books_AuthorID",
                table: "author_Books",
                newName: "IX_author_Books_AuthorID");

            migrationBuilder.AddColumn<int>(
                name: "Author_BookID",
                table: "author_Books",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_Books",
                table: "order_Books",
                column: "Order_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_author_Books",
                table: "author_Books",
                column: "Author_BookID");

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    SubCategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryID);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookInfo_SubCategoryID",
                table: "BookInfo",
                column: "SubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_author_Books_BookID",
                table: "author_Books",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryID",
                table: "SubCategories",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_author_Books_Authors_AuthorID",
                table: "author_Books",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_author_Books_BookInfo_BookID",
                table: "author_Books",
                column: "BookID",
                principalTable: "BookInfo",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookInfo_SubCategories_SubCategoryID",
                table: "BookInfo",
                column: "SubCategoryID",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_Books_BookInfo_BookID",
                table: "order_Books",
                column: "BookID",
                principalTable: "BookInfo",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_Books_Orders_OrderID",
                table: "order_Books",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
