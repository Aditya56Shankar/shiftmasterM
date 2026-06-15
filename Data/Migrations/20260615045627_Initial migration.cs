using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Departments_DepartmentID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Users_CreatedByID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_WorkLocations_LocationID",
                table: "WeeklyRosters");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "WeeklyRosters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "WeeklyRosters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByID",
                table: "WeeklyRosters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftPatternID",
                table: "ShiftAssignments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "SchedulingConstraintViolations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RosterID",
                table: "SchedulingConstraintViolations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments",
                column: "ShiftPatternID",
                principalTable: "ShiftPatterns",
                principalColumn: "PatternID");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_Departments_DepartmentID",
                table: "WeeklyRosters",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_Users_CreatedByID",
                table: "WeeklyRosters",
                column: "CreatedByID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_WorkLocations_LocationID",
                table: "WeeklyRosters",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Departments_DepartmentID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Users_CreatedByID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_WorkLocations_LocationID",
                table: "WeeklyRosters");

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

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "WeeklyRosters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "WeeklyRosters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByID",
                table: "WeeklyRosters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftPatternID",
                table: "ShiftAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "SchedulingConstraintViolations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RosterID",
                table: "SchedulingConstraintViolations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments",
                column: "ShiftPatternID",
                principalTable: "ShiftPatterns",
                principalColumn: "PatternID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_Departments_DepartmentID",
                table: "WeeklyRosters",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "departmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_Users_CreatedByID",
                table: "WeeklyRosters",
                column: "CreatedByID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_WorkLocations_LocationID",
                table: "WeeklyRosters",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
