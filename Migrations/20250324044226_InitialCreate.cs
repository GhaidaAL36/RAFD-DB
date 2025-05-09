﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAFDDB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAge = table.Column<int>(type: "int", nullable: false),
                    UserNationalID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserJopType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreditScore = table.Column<int>(type: "int", nullable: false),
                    UserContactNum = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FingerPrintData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
