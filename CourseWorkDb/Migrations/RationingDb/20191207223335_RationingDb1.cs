using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkDb.Migrations.RationingDb
{
    public partial class RationingDb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(maxLength: 50, nullable: false),
                    FormOfOwnership = table.Column<string>(maxLength: 50, nullable: false),
                    HeadName = table.Column<string>(maxLength: 50, nullable: false),
                    ActivityType = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(maxLength: 50, nullable: false),
                    ProductTypeId = table.Column<int>(nullable: false),
                    MeasureUnit = table.Column<string>(maxLength: 50, nullable: false),
                    Features = table.Column<string>(maxLength: 50, nullable: true),
                    Photo = table.Column< byte[]>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                       name: "FK_Products_ProductTypes_ProductTypeId",
                       column: x => x.ProductTypeId,
                       principalTable: "Companies",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);

                });

            migrationBuilder.CreateTable(
               name: "ProductTypes",
               columns: table => new
               {
                   Id = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   ProductionType = table.Column<string>(maxLength: 50, nullable: false),
                   
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_ProductTypes", x => x.Id);
               });

            migrationBuilder.CreateTable(
                name: "Outputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutputPlan = table.Column<int>(nullable: false),
                    OutputFact = table.Column<int>(nullable: false),
                    Quarter = table.Column<short>(nullable: false),
                    Year = table.Column<short>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outputs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Outputs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleasePlan = table.Column<int>(nullable: false),
                    ReleaseFact = table.Column<int>(nullable: false),
                    Quarter = table.Column<short>(nullable: false),
                    Year = table.Column<short>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Releases_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Releases_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_CompanyId",
                table: "Outputs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_ProductId",
                table: "Outputs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_CompanyId",
                table: "Releases",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_ProductId",
                table: "Releases",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outputs");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
