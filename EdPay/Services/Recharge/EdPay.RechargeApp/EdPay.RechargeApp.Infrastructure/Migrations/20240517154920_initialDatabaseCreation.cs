using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EdPay.RechargeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialDatabaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beneficiaries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryID = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiaries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CreditRules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AmountLimit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PerBeneficiary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RechargeHistory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RechargeHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TopupPlans",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopupPlans", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "CreditRules",
                columns: new[] { "ID", "AmountLimit", "Description", "PerBeneficiary" },
                values: new object[,]
                {
                    { 1, 1000m, "NonVerifiedUser", true },
                    { 2, 500m, "VerifiedUser", true },
                    { 3, 3000m, "VerifiedUserPerMonth", false }
                });

            migrationBuilder.InsertData(
                table: "TopupPlans",
                columns: new[] { "ID", "Amount", "Currency", "IsActive", "PlanDescription" },
                values: new object[,]
                {
                    { 1, 5, "AED", true, "AED 5" },
                    { 2, 10, "AED", true, "AED 10" },
                    { 3, 20, "AED", true, "AED 20" },
                    { 4, 30, "AED", true, "AED 30" },
                    { 5, 50, "AED", true, "AED 50" },
                    { 6, 75, "AED", true, "AED 75" },
                    { 7, 100, "AED", true, "AED 100" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beneficiaries");

            migrationBuilder.DropTable(
                name: "CreditRules");

            migrationBuilder.DropTable(
                name: "RechargeHistory");

            migrationBuilder.DropTable(
                name: "TopupPlans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
