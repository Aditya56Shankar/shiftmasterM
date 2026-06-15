using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulingConstraintViolations_WeeklyRosters_WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations");

            migrationBuilder.DropIndex(
                name: "IX_SchedulingConstraintViolations_WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations");

            migrationBuilder.DropColumn(
                name: "WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "departmentId", "departmentName" },
                values: new object[,]
                {
                    { 1, "Production" },
                    { 2, "Maintenance" },
                    { 3, "Quality Control" }
                });

            migrationBuilder.InsertData(
                table: "WorkLocations",
                columns: new[] { "LocationID", "City", "LocationName", "ManagerID", "OperatingHours", "Status", "Type" },
                values: new object[,]
                {
                    { 1, "Chennai", "Chennai Plant", null, "09:00-21:00", "Active", "Store" },
                    { 2, "Bangalore", "Bangalore Hub", null, "08:00-20:00", "Active", "Store" }
                });

            migrationBuilder.InsertData(
                table: "ShiftPatterns",
                columns: new[] { "PatternID", "BreakMinutes", "DurationHours", "EndTime", "LocationID", "MinStaffingLevel", "PatternName", "ShiftType", "StartTime", "Status" },
                values: new object[,]
                {
                    { 1, 0, 0m, new TimeSpan(0, 17, 0, 0, 0), 1, 2, "Morning Shift", "Morning", new TimeSpan(0, 9, 0, 0, 0), "Active" },
                    { 2, 0, 0m, new TimeSpan(0, 1, 0, 0, 0), 1, 2, "Evening Shift", "Afternoon", new TimeSpan(0, 17, 0, 0, 0), "Active" },
                    { 3, 0, 0m, new TimeSpan(0, 9, 0, 0, 0), 2, 1, "Night Shift", "Night", new TimeSpan(0, 1, 0, 0, 0), "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "departmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "departmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "departmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ShiftPatterns",
                keyColumn: "PatternID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShiftPatterns",
                keyColumn: "PatternID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ShiftPatterns",
                keyColumn: "PatternID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkLocations",
                keyColumn: "LocationID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkLocations",
                keyColumn: "LocationID",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchedulingConstraintViolations_WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations",
                column: "WeeklyRosterRosterID");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulingConstraintViolations_WeeklyRosters_WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations",
                column: "WeeklyRosterRosterID",
                principalTable: "WeeklyRosters",
                principalColumn: "RosterID");
        }
    }
}
