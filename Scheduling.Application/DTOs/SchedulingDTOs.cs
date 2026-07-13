using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.SchedulingService.Application.DTOs
{
    public class WeeklyRosterDto
    {
        public int RosterID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Status { get; set; } = null!;
        public int LocationID { get; set; }
        public int CreatedByID { get; set; }
        public int DepartmentID { get; set; }
        public int? ApprovedByUserID { get; set; }
        public string LocationName { get; set; } = "Unassigned";
        public string DepartmentName { get; set; } = "Unassigned";
        public List<ShiftAssignmentDto> ShiftAssignments { get; set; } = new List<ShiftAssignmentDto>();
        public List<SchedulingConstraintViolationDto> Violations { get; set; } = new List<SchedulingConstraintViolationDto>();
    }

    public class RosterResponseDto
    {
        public int RosterID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string Status { get; set; } = null!;
        public int LocationID { get; set; }
        public int CreatedByID { get; set; }
        public int DepartmentID { get; set; }
    }

    public class CreateRosterDto
    {
        [Required] public DateTime WeekStartDate { get; set; }
        [Required] public int LocationID { get; set; }
        [Required] public int DepartmentID { get; set; }
    }

    public class ShiftAssignmentDto
    {
        public int AssignmentID { get; set; }
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int RosterID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; } = "Unknown";
        public int ShiftPatternID { get; set; }
        public string PatternName { get; set; } = "None";
    }

    public class CreateAssignmentDto
    {
        [Required(ErrorMessage = "Roster ID container reference is required.")]
        public int RosterID { get; set; }

        [Required(ErrorMessage = "Target employee UserID is required.")]
        public int UserID { get; set; }

        public int? ShiftPatternID { get; set; }

        [Required(ErrorMessage = "Shift calendar date is required.")]
        public DateTime AssignedDate { get; set; }

        [Required(ErrorMessage = "Shift start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Shift end time is required.")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Assigned functional role is required.")]
        [StringLength(100, ErrorMessage = "Role cannot exceed 100 characters.")]
        public string Role { get; set; } = null!;
    }

    public class AssignmentResponseDto
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public int RosterID { get; set; }
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class SchedulingConstraintViolationDto
    {
        public int ViolationID { get; set; }
        public string ViolationType { get; set; } = null!;
        public string Severity { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int RosterID { get; set; }
        public int? UserID { get; set; }
        public string EmployeeName { get; set; } = "Unknown";
    }

    public class ShiftPatternDto
    {
        public int PatternID { get; set; }
        public string PatternName { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal DurationHours { get; set; }
        public int BreakMinutes { get; set; }
        public string ShiftType { get; set; } = null!;
        public int MinStaffingLevel { get; set; }
        public string Status { get; set; } = null!;
        public int LocationID { get; set; }
        public string LocationName { get; set; } = "Unassigned";
    }

    public class CreateShiftPatternDto
    {
        [Required, MaxLength(100)] public string PatternName { get; set; } = null!;
        [Required] public string StartTime { get; set; } = null!;
        [Required] public string EndTime { get; set; } = null!;
        [Required] public decimal DurationHours { get; set; }
        [Required] public int BreakMinutes { get; set; }
        [Required] public string ShiftType { get; set; } = null!;
        [Required] public int MinStaffingLevel { get; set; }
        [Required] public string Status { get; set; } = null!;
        [Required] public int LocationID { get; set; }
    }

    public class UpdateShiftPatternDto
    {
        [Required, MaxLength(100)] public string PatternName { get; set; } = null!;
        [Required] public string StartTime { get; set; } = null!;
        [Required] public string EndTime { get; set; } = null!;
        [Required] public decimal DurationHours { get; set; }
        [Required] public int BreakMinutes { get; set; }
        [Required] public string ShiftType { get; set; } = null!;
        [Required] public int MinStaffingLevel { get; set; }
        [Required] public string Status { get; set; } = null!;
        [Required] public int LocationID { get; set; }
    }

    public class SwapRequestResponseDto
    {
        public int SwapID { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int RequesterUserID { get; set; }
        public int TargetUserID { get; set; }
        public int OriginalAssignmentID { get; set; }
        public int? ProposedAssignmentID { get; set; }
        public int? ApprovedByID { get; set; }
    }

    public class CreateSwapRequestDto
    {
        [Required, MaxLength(500)]
        public string Reason { get; set; } = null!;
        [Required] public int RequesterUserID { get; set; }
        [Required] public int TargetUserID { get; set; }
        [Required] public int OriginalAssignmentID { get; set; }
        public int? ProposedAssignmentID { get; set; }
    }

    public class SwapEligibilityDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<SwapAssignmentOptionDto> AvailableAssignments { get; set; } = new List<SwapAssignmentOptionDto>();
    }

    public class SwapAssignmentOptionDto
    {
        public int AssignmentID { get; set; }
        public DateTime AssignedDate { get; set; }
    }

    public class CreateCoverAssignmentDto
    {
        [Range(1, int.MaxValue)]
        public int OriginalAssignmentID { get; set; }

        [Range(1, int.MaxValue)]
        public int CoveringUserID { get; set; }

        [Range(1, int.MaxValue)]
        public int AssignedByID { get; set; }

        public string CoverType { get; set; } = null!;
        public bool OvertimeApplicable { get; set; }
    }

    public class CoverAssignmentResponseDto
    {
        public int CoverID { get; set; }
        public int OriginalAssignmentID { get; set; }
        public int CoveringUserID { get; set; }
        public int AssignedByID { get; set; }
        public string CoverType { get; set; } = null!;
        public bool OvertimeApplicable { get; set; }
        public string Status { get; set; } = null!;
    }

    public class CoverEligibilityDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<string> MatchingSkills { get; set; } = new List<string>();
    }

    public class SupervisorRosterResponseDto
    {
        public int RosterID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string Status { get; set; } = null!;
        public int? CreatedByID { get; set; }
        public DateTime? PublishedDate { get; set; }

        public ICollection<SupervisorAssignmentViewDto> ShiftAssignments { get; set; } = new List<SupervisorAssignmentViewDto>();
        public ICollection<ViolationViewDto> Violations { get; set; } = new List<ViolationViewDto>();
    }

    public class SupervisorAssignmentViewDto
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; } = "Unknown Employee";
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class ViolationViewDto
    {
        public int ViolationID { get; set; }
        public int UserID { get; set; }
        public string ViolationType { get; set; } = null!;
        public string Severity { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    // Proxy DTOs for employee info
    public class EmployeeFullDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public List<AvailabilityDto> Availability { get; set; } = new List<AvailabilityDto>();
        public List<EmployeeSkillDto> Skills { get; set; } = new List<EmployeeSkillDto>();
    }

    public class AvailabilityDto
    {
        public int AvailabilityID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string AvailableDays { get; set; } = null!;
        public string? PreferredShiftType { get; set; }
        public decimal MaxHoursPerWeek { get; set; }
        public string Status { get; set; } = null!;
        public int UserID { get; set; }
    }

    public class EmployeeSkillDto
    {
        public int EmpSkillID { get; set; }
        public string SkillName { get; set; } = null!;
        public string ProficiencyLevel { get; set; } = null!;
        public DateTime CertifiedDate { get; set; }
        public string Status { get; set; } = null!;
        public int UserID { get; set; }
    }

    // Identical DTO models from other services
    public class LogLoginDto
    {
        public int? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string AuthMethod { get; set; } = "Password";
        public string? CorrelationId { get; set; }
        public string Source { get; set; } = "Web";
        public string? Details { get; set; }
        public string? ClientAppVersion { get; set; }
    }

    public class LogRegistrationDto
    {
        public int? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string? CorrelationId { get; set; }
        public string Source { get; set; } = "Web";
        public string? Details { get; set; }
        public string? ClientAppVersion { get; set; }
    }

    public class LogEventDto
    {
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public int? RecordId { get; set; }
        public int? UserId { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public int StatusCode { get; set; }
        public string? Details { get; set; }
    }

    public class RespondToSwapDto
    {
        public bool Accepted { get; set; }
    }

    public class ApproveSwapDto
    {
        public bool Approved { get; set; }
    }
}
