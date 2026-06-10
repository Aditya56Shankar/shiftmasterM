using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_ShiftAssignments_AssignmentID",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Users_UserID",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_UserID",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilitySubmissions_Users_UserID",
                table: "AvailabilitySubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_CoverAssignments_ShiftAssignments_OriginalAssignmentID",
                table: "CoverAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkills_Users_UserID",
                table: "EmployeeSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftPatterns_WorkLocations_LocationID",
                table: "ShiftPatterns");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillRequirements_WorkLocations_LocationID",
                table: "SkillRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Users_CreatedByID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_WorkLocations_LocationID",
                table: "WeeklyRosters");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerID",
                table: "WorkLocations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "WorkLocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "WeeklyRosters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "WeeklyRosters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "TimesheetSummaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SwapRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "SkillRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SkillRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ShiftPatterns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ShiftAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SchedulingConstraintViolations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "OvertimeAuthorisations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "LeaveBlocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "LabourReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EmployeeSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "CoverAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AvailabilitySubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AttendanceRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.departmentId);
                    table.ForeignKey(
                        name: "FK_Departments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleId);
                    table.ForeignKey(
                        name: "FK_Roles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocations_TenantId",
                table: "WorkLocations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyRosters_DepartmentID",
                table: "WeeklyRosters",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyRosters_TenantId",
                table: "WeeklyRosters",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentID",
                table: "Users",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_EmployeeID",
                table: "Users",
                columns: new[] { "TenantId", "EmployeeID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetSummaries_TenantId",
                table: "TimesheetSummaries",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_TenantId",
                table: "SwapRequests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillRequirements_DepartmentID",
                table: "SkillRequirements",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_SkillRequirements_TenantId",
                table: "SkillRequirements",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftPatterns_TenantId",
                table: "ShiftPatterns",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_TenantId",
                table: "ShiftAssignments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulingConstraintViolations_TenantId",
                table: "SchedulingConstraintViolations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeAuthorisations_TenantId",
                table: "OvertimeAuthorisations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TenantId",
                table: "Notifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBlocks_TenantId",
                table: "LeaveBlocks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LabourReports_TenantId",
                table: "LabourReports",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_TenantId",
                table: "EmployeeSkills",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverAssignments_TenantId",
                table: "CoverAssignments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySubmissions_TenantId",
                table: "AvailabilitySubmissions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TenantId",
                table: "AuditLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_TenantId",
                table: "AttendanceRecords",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TenantId",
                table: "Departments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId",
                table: "Roles",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_ShiftAssignments_AssignmentID",
                table: "AttendanceRecords",
                column: "AssignmentID",
                principalTable: "ShiftAssignments",
                principalColumn: "AssignmentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Tenants_TenantId",
                table: "AttendanceRecords",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Users_UserID",
                table: "AttendanceRecords",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Tenants_TenantId",
                table: "AuditLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_UserID",
                table: "AuditLogs",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilitySubmissions_Tenants_TenantId",
                table: "AvailabilitySubmissions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilitySubmissions_Users_UserID",
                table: "AvailabilitySubmissions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoverAssignments_ShiftAssignments_OriginalAssignmentID",
                table: "CoverAssignments",
                column: "OriginalAssignmentID",
                principalTable: "ShiftAssignments",
                principalColumn: "AssignmentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoverAssignments_Tenants_TenantId",
                table: "CoverAssignments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkills_Tenants_TenantId",
                table: "EmployeeSkills",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkills_Users_UserID",
                table: "EmployeeSkills",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabourReports_Tenants_TenantId",
                table: "LabourReports",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveBlocks_Tenants_TenantId",
                table: "LeaveBlocks",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Tenants_TenantId",
                table: "Notifications",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeAuthorisations_Tenants_TenantId",
                table: "OvertimeAuthorisations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulingConstraintViolations_Tenants_TenantId",
                table: "SchedulingConstraintViolations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments",
                column: "ShiftPatternID",
                principalTable: "ShiftPatterns",
                principalColumn: "PatternID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_Tenants_TenantId",
                table: "ShiftAssignments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftPatterns_Tenants_TenantId",
                table: "ShiftPatterns",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftPatterns_WorkLocations_LocationID",
                table: "ShiftPatterns",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillRequirements_Departments_DepartmentID",
                table: "SkillRequirements",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "departmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillRequirements_Tenants_TenantId",
                table: "SkillRequirements",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillRequirements_WorkLocations_LocationID",
                table: "SkillRequirements",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SwapRequests_Tenants_TenantId",
                table: "SwapRequests",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetSummaries_Tenants_TenantId",
                table: "TimesheetSummaries",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentID",
                table: "Users",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "departmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "roleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_Departments_DepartmentID",
                table: "WeeklyRosters",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "departmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_Tenants_TenantId",
                table: "WeeklyRosters",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocations_Tenants_TenantId",
                table: "WorkLocations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_ShiftAssignments_AssignmentID",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Tenants_TenantId",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Users_UserID",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Tenants_TenantId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_UserID",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilitySubmissions_Tenants_TenantId",
                table: "AvailabilitySubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilitySubmissions_Users_UserID",
                table: "AvailabilitySubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_CoverAssignments_ShiftAssignments_OriginalAssignmentID",
                table: "CoverAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CoverAssignments_Tenants_TenantId",
                table: "CoverAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkills_Tenants_TenantId",
                table: "EmployeeSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkills_Users_UserID",
                table: "EmployeeSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_LabourReports_Tenants_TenantId",
                table: "LabourReports");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveBlocks_Tenants_TenantId",
                table: "LeaveBlocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Tenants_TenantId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeAuthorisations_Tenants_TenantId",
                table: "OvertimeAuthorisations");

            migrationBuilder.DropForeignKey(
                name: "FK_SchedulingConstraintViolations_Tenants_TenantId",
                table: "SchedulingConstraintViolations");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_Tenants_TenantId",
                table: "ShiftAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftPatterns_Tenants_TenantId",
                table: "ShiftPatterns");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftPatterns_WorkLocations_LocationID",
                table: "ShiftPatterns");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillRequirements_Departments_DepartmentID",
                table: "SkillRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillRequirements_Tenants_TenantId",
                table: "SkillRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillRequirements_WorkLocations_LocationID",
                table: "SkillRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_SwapRequests_Tenants_TenantId",
                table: "SwapRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetSummaries_Tenants_TenantId",
                table: "TimesheetSummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Departments_DepartmentID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Tenants_TenantId",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_Users_CreatedByID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyRosters_WorkLocations_LocationID",
                table: "WeeklyRosters");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocations_Tenants_TenantId",
                table: "WorkLocations");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocations_TenantId",
                table: "WorkLocations");

            migrationBuilder.DropIndex(
                name: "IX_WeeklyRosters_DepartmentID",
                table: "WeeklyRosters");

            migrationBuilder.DropIndex(
                name: "IX_WeeklyRosters_TenantId",
                table: "WeeklyRosters");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId_EmployeeID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TimesheetSummaries_TenantId",
                table: "TimesheetSummaries");

            migrationBuilder.DropIndex(
                name: "IX_SwapRequests_TenantId",
                table: "SwapRequests");

            migrationBuilder.DropIndex(
                name: "IX_SkillRequirements_DepartmentID",
                table: "SkillRequirements");

            migrationBuilder.DropIndex(
                name: "IX_SkillRequirements_TenantId",
                table: "SkillRequirements");

            migrationBuilder.DropIndex(
                name: "IX_ShiftPatterns_TenantId",
                table: "ShiftPatterns");

            migrationBuilder.DropIndex(
                name: "IX_ShiftAssignments_TenantId",
                table: "ShiftAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SchedulingConstraintViolations_TenantId",
                table: "SchedulingConstraintViolations");

            migrationBuilder.DropIndex(
                name: "IX_OvertimeAuthorisations_TenantId",
                table: "OvertimeAuthorisations");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TenantId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_LeaveBlocks_TenantId",
                table: "LeaveBlocks");

            migrationBuilder.DropIndex(
                name: "IX_LabourReports_TenantId",
                table: "LabourReports");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSkills_TenantId",
                table: "EmployeeSkills");

            migrationBuilder.DropIndex(
                name: "IX_CoverAssignments_TenantId",
                table: "CoverAssignments");

            migrationBuilder.DropIndex(
                name: "IX_AvailabilitySubmissions_TenantId",
                table: "AvailabilitySubmissions");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_TenantId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_TenantId",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "WorkLocations");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "WeeklyRosters");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "WeeklyRosters");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "TimesheetSummaries");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SwapRequests");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "SkillRequirements");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SkillRequirements");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ShiftAssignments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SchedulingConstraintViolations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "OvertimeAuthorisations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "LeaveBlocks");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "LabourReports");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EmployeeSkills");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CoverAssignments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AvailabilitySubmissions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AttendanceRecords");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerID",
                table: "WorkLocations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_ShiftAssignments_AssignmentID",
                table: "AttendanceRecords",
                column: "AssignmentID",
                principalTable: "ShiftAssignments",
                principalColumn: "AssignmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Users_UserID",
                table: "AttendanceRecords",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_UserID",
                table: "AuditLogs",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilitySubmissions_Users_UserID",
                table: "AvailabilitySubmissions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoverAssignments_ShiftAssignments_OriginalAssignmentID",
                table: "CoverAssignments",
                column: "OriginalAssignmentID",
                principalTable: "ShiftAssignments",
                principalColumn: "AssignmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkills_Users_UserID",
                table: "EmployeeSkills",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments",
                column: "ShiftPatternID",
                principalTable: "ShiftPatterns",
                principalColumn: "PatternID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftPatterns_WorkLocations_LocationID",
                table: "ShiftPatterns",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillRequirements_WorkLocations_LocationID",
                table: "SkillRequirements",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_Users_CreatedByID",
                table: "WeeklyRosters",
                column: "CreatedByID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyRosters_WorkLocations_LocationID",
                table: "WeeklyRosters",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
