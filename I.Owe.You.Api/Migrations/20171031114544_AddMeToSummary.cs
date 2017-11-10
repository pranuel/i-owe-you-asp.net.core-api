using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace I.Owe.You.Api.Migrations
{
    public partial class AddMeToSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeId",
                table: "DebtsSummaries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DebtsSummaries_MeId",
                table: "DebtsSummaries",
                column: "MeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtsSummaries_Users_MeId",
                table: "DebtsSummaries",
                column: "MeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtsSummaries_Users_MeId",
                table: "DebtsSummaries");

            migrationBuilder.DropIndex(
                name: "IX_DebtsSummaries_MeId",
                table: "DebtsSummaries");

            migrationBuilder.DropColumn(
                name: "MeId",
                table: "DebtsSummaries");
        }
    }
}
