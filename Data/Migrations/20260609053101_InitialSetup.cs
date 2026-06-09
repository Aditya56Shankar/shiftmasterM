using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClockOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BreakMinutesTaken = table.Column<int>(type: "int", nullable: false),
                    ActualHoursWorked = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    VarianceMinutes = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AssignmentID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.AttendanceID);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecordID = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditID);
                });

            migrationBuilder.CreateTable(
                name: "AvailabilitySubmissions",
                columns: table => new
                {
                    AvailabilityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableDays = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PreferredShiftType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxHoursPerWeek = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilitySubmissions", x => x.AvailabilityID);
                });

            migrationBuilder.CreateTable(
                name: "CoverAssignments",
                columns: table => new
                {
                    CoverID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoverType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OvertimeApplicable = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OriginalAssignmentID = table.Column<int>(type: "int", nullable: false),
                    CoveringUserID = table.Column<int>(type: "int", nullable: false),
                    AssignedByID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverAssignments", x => x.CoverID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkills",
                columns: table => new
                {
                    EmpSkillID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProficiencyLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CertifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkills", x => x.EmpSkillID);
                });

            migrationBuilder.CreateTable(
                name: "LabourReports",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scope = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Metrics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeneratedByID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabourReports", x => x.ReportID);
                });

            migrationBuilder.CreateTable(
                name: "LeaveBlocks",
                columns: table => new
                {
                    LeaveBlockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ApprovedByID = table.Column<int>(type: "int", nullable: true),
                    UserID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveBlocks", x => x.LeaveBlockID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationID);
                });

            migrationBuilder.CreateTable(
                name: "OvertimeAuthorisations",
                columns: table => new
                {
                    OTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedOTHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ActualOTHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OTType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AuthorisedByID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeAuthorisations", x => x.OTID);
                });

            migrationBuilder.CreateTable(
                name: "SchedulingConstraintViolations",
                columns: table => new
                {
                    ViolationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViolationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RosterID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    WeeklyRosterRosterID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulingConstraintViolations", x => x.ViolationID);
                });

            migrationBuilder.CreateTable(
                name: "ShiftAssignments",
                columns: table => new
                {
                    AssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RosterID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ShiftPatternID = table.Column<int>(type: "int", nullable: false),
                    UserID1 = table.Column<int>(type: "int", nullable: true),
                    WeeklyRosterRosterID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftAssignments", x => x.AssignmentID);
                });

            migrationBuilder.CreateTable(
                name: "ShiftPatterns",
                columns: table => new
                {
                    PatternID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatternName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DurationHours = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    BreakMinutes = table.Column<int>(type: "int", nullable: false),
                    ShiftType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinStaffingLevel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftPatterns", x => x.PatternID);
                });

            migrationBuilder.CreateTable(
                name: "SkillRequirements",
                columns: table => new
                {
                    SkillReqID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinCountPerShift = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillRequirements", x => x.SkillReqID);
                });

            migrationBuilder.CreateTable(
                name: "SwapRequests",
                columns: table => new
                {
                    SwapID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequesterUserID = table.Column<int>(type: "int", nullable: false),
                    TargetUserID = table.Column<int>(type: "int", nullable: false),
                    OriginalAssignmentID = table.Column<int>(type: "int", nullable: false),
                    ProposedAssignmentID = table.Column<int>(type: "int", nullable: true),
                    ApprovedByID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapRequests", x => x.SwapID);
                    table.ForeignKey(
                        name: "FK_SwapRequests_ShiftAssignments_OriginalAssignmentID",
                        column: x => x.OriginalAssignmentID,
                        principalTable: "ShiftAssignments",
                        principalColumn: "AssignmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwapRequests_ShiftAssignments_ProposedAssignmentID",
                        column: x => x.ProposedAssignmentID,
                        principalTable: "ShiftAssignments",
                        principalColumn: "AssignmentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetSummaries",
                columns: table => new
                {
                    TimesheetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegularHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OvertimeHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PublicHolidayHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ApprovedByID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetSummaries", x => x.TimesheetID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "WorkLocations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OperatingHours = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLocations", x => x.LocationID);
                    table.ForeignKey(
                        name: "FK_WorkLocations_Users_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyRosters",
                columns: table => new
                {
                    RosterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeekEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    CreatedByID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyRosters", x => x.RosterID);
                    table.ForeignKey(
                        name: "FK_WeeklyRosters_Users_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklyRosters_WorkLocations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "WorkLocations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_AssignmentID",
                table: "AttendanceRecords",
                column: "AssignmentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_UserID",
                table: "AttendanceRecords",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserID",
                table: "AuditLogs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySubmissions_UserID",
                table: "AvailabilitySubmissions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CoverAssignments_AssignedByID",
                table: "CoverAssignments",
                column: "AssignedByID");

            migrationBuilder.CreateIndex(
                name: "IX_CoverAssignments_CoveringUserID",
                table: "CoverAssignments",
                column: "CoveringUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CoverAssignments_OriginalAssignmentID",
                table: "CoverAssignments",
                column: "OriginalAssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_UserID",
                table: "EmployeeSkills",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_LabourReports_GeneratedByID",
                table: "LabourReports",
                column: "GeneratedByID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBlocks_ApprovedByID",
                table: "LeaveBlocks",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBlocks_UserID",
                table: "LeaveBlocks",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBlocks_UserID1",
                table: "LeaveBlocks",
                column: "UserID1");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserID",
                table: "Notifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeAuthorisations_AuthorisedByID",
                table: "OvertimeAuthorisations",
                column: "AuthorisedByID");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeAuthorisations_UserID",
                table: "OvertimeAuthorisations",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulingConstraintViolations_RosterID",
                table: "SchedulingConstraintViolations",
                column: "RosterID");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulingConstraintViolations_UserID",
                table: "SchedulingConstraintViolations",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulingConstraintViolations_WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations",
                column: "WeeklyRosterRosterID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_RosterID",
                table: "ShiftAssignments",
                column: "RosterID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_ShiftPatternID",
                table: "ShiftAssignments",
                column: "ShiftPatternID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_UserID",
                table: "ShiftAssignments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_UserID1",
                table: "ShiftAssignments",
                column: "UserID1");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_WeeklyRosterRosterID",
                table: "ShiftAssignments",
                column: "WeeklyRosterRosterID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftPatterns_LocationID",
                table: "ShiftPatterns",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_SkillRequirements_LocationID",
                table: "SkillRequirements",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_ApprovedByID",
                table: "SwapRequests",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_OriginalAssignmentID",
                table: "SwapRequests",
                column: "OriginalAssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_ProposedAssignmentID",
                table: "SwapRequests",
                column: "ProposedAssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_RequesterUserID",
                table: "SwapRequests",
                column: "RequesterUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_TargetUserID",
                table: "SwapRequests",
                column: "TargetUserID");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetSummaries_ApprovedByID",
                table: "TimesheetSummaries",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetSummaries_UserID",
                table: "TimesheetSummaries",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocationID",
                table: "Users",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyRosters_CreatedByID",
                table: "WeeklyRosters",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyRosters_LocationID",
                table: "WeeklyRosters",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocations_ManagerID",
                table: "WorkLocations",
                column: "ManagerID");

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
                name: "FK_CoverAssignments_Users_AssignedByID",
                table: "CoverAssignments",
                column: "AssignedByID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoverAssignments_Users_CoveringUserID",
                table: "CoverAssignments",
                column: "CoveringUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkills_Users_UserID",
                table: "EmployeeSkills",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabourReports_Users_GeneratedByID",
                table: "LabourReports",
                column: "GeneratedByID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveBlocks_Users_ApprovedByID",
                table: "LeaveBlocks",
                column: "ApprovedByID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveBlocks_Users_UserID",
                table: "LeaveBlocks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveBlocks_Users_UserID1",
                table: "LeaveBlocks",
                column: "UserID1",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeAuthorisations_Users_AuthorisedByID",
                table: "OvertimeAuthorisations",
                column: "AuthorisedByID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeAuthorisations_Users_UserID",
                table: "OvertimeAuthorisations",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulingConstraintViolations_Users_UserID",
                table: "SchedulingConstraintViolations",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulingConstraintViolations_WeeklyRosters_RosterID",
                table: "SchedulingConstraintViolations",
                column: "RosterID",
                principalTable: "WeeklyRosters",
                principalColumn: "RosterID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulingConstraintViolations_WeeklyRosters_WeeklyRosterRosterID",
                table: "SchedulingConstraintViolations",
                column: "WeeklyRosterRosterID",
                principalTable: "WeeklyRosters",
                principalColumn: "RosterID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_ShiftPatterns_ShiftPatternID",
                table: "ShiftAssignments",
                column: "ShiftPatternID",
                principalTable: "ShiftPatterns",
                principalColumn: "PatternID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_Users_UserID",
                table: "ShiftAssignments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_Users_UserID1",
                table: "ShiftAssignments",
                column: "UserID1",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_WeeklyRosters_RosterID",
                table: "ShiftAssignments",
                column: "RosterID",
                principalTable: "WeeklyRosters",
                principalColumn: "RosterID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_WeeklyRosters_WeeklyRosterRosterID",
                table: "ShiftAssignments",
                column: "WeeklyRosterRosterID",
                principalTable: "WeeklyRosters",
                principalColumn: "RosterID");

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
                name: "FK_SwapRequests_Users_ApprovedByID",
                table: "SwapRequests",
                column: "ApprovedByID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SwapRequests_Users_RequesterUserID",
                table: "SwapRequests",
                column: "RequesterUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SwapRequests_Users_TargetUserID",
                table: "SwapRequests",
                column: "TargetUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetSummaries_Users_ApprovedByID",
                table: "TimesheetSummaries",
                column: "ApprovedByID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetSummaries_Users_UserID",
                table: "TimesheetSummaries",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_WorkLocations_LocationID",
                table: "Users",
                column: "LocationID",
                principalTable: "WorkLocations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocations_Users_ManagerID",
                table: "WorkLocations");

            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "AvailabilitySubmissions");

            migrationBuilder.DropTable(
                name: "CoverAssignments");

            migrationBuilder.DropTable(
                name: "EmployeeSkills");

            migrationBuilder.DropTable(
                name: "LabourReports");

            migrationBuilder.DropTable(
                name: "LeaveBlocks");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OvertimeAuthorisations");

            migrationBuilder.DropTable(
                name: "SchedulingConstraintViolations");

            migrationBuilder.DropTable(
                name: "SkillRequirements");

            migrationBuilder.DropTable(
                name: "SwapRequests");

            migrationBuilder.DropTable(
                name: "TimesheetSummaries");

            migrationBuilder.DropTable(
                name: "ShiftAssignments");

            migrationBuilder.DropTable(
                name: "ShiftPatterns");

            migrationBuilder.DropTable(
                name: "WeeklyRosters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkLocations");
        }
    }
}
