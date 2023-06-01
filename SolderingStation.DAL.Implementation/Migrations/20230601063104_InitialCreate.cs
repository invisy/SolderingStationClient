using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolderingStation.DAL.Implementation.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnglishName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NativeName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LanguageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerialConnectionsParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PortName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BaudRate = table.Column<int>(type: "INTEGER", nullable: false),
                    Parity = table.Column<int>(type: "INTEGER", nullable: false),
                    DataBits = table.Column<int>(type: "INTEGER", nullable: false),
                    StopBits = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialConnectionsParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SerialConnectionsParameters_Profiles_ProfileEntityId",
                        column: x => x.ProfileEntityId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThermalProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ProfileEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThermalProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThermalProfiles_Profiles_ProfileEntityId",
                        column: x => x.ProfileEntityId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThermalProfileParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ThermalProfileEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThermalProfileParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThermalProfileParts_ThermalProfiles_ThermalProfileEntityId",
                        column: x => x.ThermalProfileEntityId,
                        principalTable: "ThermalProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TemperatureMeasurementPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SecondsElapsed = table.Column<float>(type: "REAL", nullable: false),
                    Temperature = table.Column<ushort>(type: "INTEGER", nullable: false),
                    ThermalProfilePartEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureMeasurementPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemperatureMeasurementPoints_ThermalProfileParts_ThermalProfilePartEntityId",
                        column: x => x.ThermalProfilePartEntityId,
                        principalTable: "ThermalProfileParts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_LanguageId",
                table: "Profiles",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialConnectionsParameters_ProfileEntityId",
                table: "SerialConnectionsParameters",
                column: "ProfileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureMeasurementPoints_ThermalProfilePartEntityId",
                table: "TemperatureMeasurementPoints",
                column: "ThermalProfilePartEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ThermalProfileParts_ThermalProfileEntityId",
                table: "ThermalProfileParts",
                column: "ThermalProfileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ThermalProfiles_ProfileEntityId",
                table: "ThermalProfiles",
                column: "ProfileEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerialConnectionsParameters");

            migrationBuilder.DropTable(
                name: "TemperatureMeasurementPoints");

            migrationBuilder.DropTable(
                name: "ThermalProfileParts");

            migrationBuilder.DropTable(
                name: "ThermalProfiles");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
