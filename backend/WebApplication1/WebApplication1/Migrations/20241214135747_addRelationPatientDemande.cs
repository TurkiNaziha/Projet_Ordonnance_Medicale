using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class addRelationPatientDemande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatID",
                table: "Demandes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Demandes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Demandes_PatID",
                table: "Demandes",
                column: "PatID");

            migrationBuilder.CreateIndex(
                name: "IX_Demandes_PatientId",
                table: "Demandes",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Patients_PatID",
                table: "Demandes",
                column: "PatID",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Patients_PatientId",
                table: "Demandes",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Patients_PatID",
                table: "Demandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Patients_PatientId",
                table: "Demandes");

            migrationBuilder.DropIndex(
                name: "IX_Demandes_PatID",
                table: "Demandes");

            migrationBuilder.DropIndex(
                name: "IX_Demandes_PatientId",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "PatID",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Demandes");
        }
    }
}
