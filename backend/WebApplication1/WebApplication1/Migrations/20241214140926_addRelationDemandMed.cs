using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class addRelationDemandMed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
