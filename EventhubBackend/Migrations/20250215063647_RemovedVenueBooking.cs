using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventhub.Migrations
{
    /// <inheritdoc />
    public partial class RemovedVenueBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_VenueBooking_VenueBookingBookingId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "VenueBooking");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_VenueBookingBookingId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "VenueBookingBookingId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BookingId",
                table: "Tickets",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Bookings_BookingId",
                table: "Tickets",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Bookings_BookingId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_BookingId",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "VenueBookingBookingId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VenueBooking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueBooking", x => x.BookingId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_VenueBookingBookingId",
                table: "Tickets",
                column: "VenueBookingBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_VenueBooking_VenueBookingBookingId",
                table: "Tickets",
                column: "VenueBookingBookingId",
                principalTable: "VenueBooking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
