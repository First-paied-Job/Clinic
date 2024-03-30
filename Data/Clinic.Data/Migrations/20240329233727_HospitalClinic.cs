using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Data.Migrations
{
    /// <inheritdoc />
    public partial class HospitalClinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clincs_Hospitals_HospitalEmployerId",
                table: "Clincs");

            migrationBuilder.AlterColumn<string>(
                name: "HospitalEmployerId",
                table: "Clincs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HospitalEmployerHospitalId",
                table: "Clincs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clincs_HospitalEmployerHospitalId",
                table: "Clincs",
                column: "HospitalEmployerHospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clincs_Hospitals_HospitalEmployerHospitalId",
                table: "Clincs",
                column: "HospitalEmployerHospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clincs_Hospitals_HospitalEmployerId",
                table: "Clincs",
                column: "HospitalEmployerId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clincs_Hospitals_HospitalEmployerHospitalId",
                table: "Clincs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clincs_Hospitals_HospitalEmployerId",
                table: "Clincs");

            migrationBuilder.DropIndex(
                name: "IX_Clincs_HospitalEmployerHospitalId",
                table: "Clincs");

            migrationBuilder.DropColumn(
                name: "HospitalEmployerHospitalId",
                table: "Clincs");

            migrationBuilder.AlterColumn<string>(
                name: "HospitalEmployerId",
                table: "Clincs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Clincs_Hospitals_HospitalEmployerId",
                table: "Clincs",
                column: "HospitalEmployerId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId");
        }
    }
}
