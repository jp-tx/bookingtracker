using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Neighborhood = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    VenueType = table.Column<int>(type: "INTEGER", nullable: false),
                    Website = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Instagram = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Facebook = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    BookingContactName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    BookingContactEmail = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    BookingContactPhone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    BookingFormUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OtherContactDetails = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    PreferredContactMethod = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CapacityRoomSize = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MusicFormatFit = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    TypicalMusicDaysTimes = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PayNotes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    HouseSound = table.Column<int>(type: "INTEGER", nullable: false),
                    TravelDistanceFromNashville = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FitScore = table.Column<int>(type: "INTEGER", nullable: true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    ResearchConfidence = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    NextAction = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NextFollowUpDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastContactedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastResponseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Owner = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutreachLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VenueId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Channel = table.Column<int>(type: "INTEGER", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderContact = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Result = table.Column<int>(type: "INTEGER", nullable: true),
                    FollowUpDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MessageDraftLink = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutreachLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutreachLogs_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutreachLogs_VenueId",
                table: "OutreachLogs",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutreachLogs");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
