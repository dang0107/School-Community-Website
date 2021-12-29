using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment1.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "community",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Budget = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "advertisement",
                columns: table => new
                {
                    AdsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunityID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertisement", x => x.AdsId);
                    table.ForeignKey(
                        name: "FK_advertisement_community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommunityMemberships",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    CommunityID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMemberships", x => new { x.StudentID, x.CommunityID });
                    table.ForeignKey(
                        name: "FK_CommunityMemberships_community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMemberships_student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_advertisement_CommunityID",
                table: "advertisement",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMemberships_CommunityID",
                table: "CommunityMemberships",
                column: "CommunityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertisement");

            migrationBuilder.DropTable(
                name: "CommunityMemberships");

            migrationBuilder.DropTable(
                name: "community");

            migrationBuilder.DropTable(
                name: "student");
        }
    }
}
