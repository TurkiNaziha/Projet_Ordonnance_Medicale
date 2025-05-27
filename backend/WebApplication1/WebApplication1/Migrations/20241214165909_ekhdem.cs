using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ekhdem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Patients_PatientId",
                table: "Demandes");

            migrationBuilder.DropIndex(
                name: "IX_Demandes_PatientId",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Demandes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Demandes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Demandes_PatientId",
                table: "Demandes",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Patients_PatientId",
                table: "Demandes",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
