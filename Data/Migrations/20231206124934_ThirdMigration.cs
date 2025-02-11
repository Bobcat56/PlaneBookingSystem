using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_flightSeatings_Flights_FlightIdFK",
                table: "flightSeatings");

            migrationBuilder.DropForeignKey(
                name: "FK_flightSeatings_Tickets_TicketIdFK",
                table: "flightSeatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_flightSeatings",
                table: "flightSeatings");

            migrationBuilder.RenameTable(
                name: "flightSeatings",
                newName: "FlightSeatings");

            migrationBuilder.RenameIndex(
                name: "IX_flightSeatings_TicketIdFK",
                table: "FlightSeatings",
                newName: "IX_FlightSeatings_TicketIdFK");

            migrationBuilder.RenameIndex(
                name: "IX_flightSeatings_FlightIdFK",
                table: "FlightSeatings",
                newName: "IX_FlightSeatings_FlightIdFK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightSeatings",
                table: "FlightSeatings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSeatings_Flights_FlightIdFK",
                table: "FlightSeatings",
                column: "FlightIdFK",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightSeatings_Tickets_TicketIdFK",
                table: "FlightSeatings",
                column: "TicketIdFK",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightSeatings_Flights_FlightIdFK",
                table: "FlightSeatings");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightSeatings_Tickets_TicketIdFK",
                table: "FlightSeatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightSeatings",
                table: "FlightSeatings");

            migrationBuilder.RenameTable(
                name: "FlightSeatings",
                newName: "flightSeatings");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSeatings_TicketIdFK",
                table: "flightSeatings",
                newName: "IX_flightSeatings_TicketIdFK");

            migrationBuilder.RenameIndex(
                name: "IX_FlightSeatings_FlightIdFK",
                table: "flightSeatings",
                newName: "IX_flightSeatings_FlightIdFK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_flightSeatings",
                table: "flightSeatings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_flightSeatings_Flights_FlightIdFK",
                table: "flightSeatings",
                column: "FlightIdFK",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_flightSeatings_Tickets_TicketIdFK",
                table: "flightSeatings",
                column: "TicketIdFK",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
