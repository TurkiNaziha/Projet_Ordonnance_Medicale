using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class medicamentDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Demandes_DemandId",
                table: "Medicaments");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_DemandId",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "DemandId",
                table: "Medicaments");

            migrationBuilder.CreateTable(
                name: "MedicamentDTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    Prix = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DemandId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentDTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MedicamentDTO_Demandes_DemandId",
                        column: x => x.DemandId,
                        principalTable: "Demandes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentDTO_DemandId",
                table: "MedicamentDTO",
                column: "DemandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicamentDTO");

            migrationBuilder.AddColumn<int>(
                name: "DemandId",
                table: "Medicaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_DemandId",
                table: "Medicaments",
                column: "DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Demandes_DemandId",
                table: "Medicaments",
                column: "DemandId",
                principalTable: "Demandes",
                principalColumn: "Id");
        }
    }
}
