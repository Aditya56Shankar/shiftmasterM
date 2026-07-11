using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShiftMaster.Employee.DTOs
{
    public class LeaveBlockRequestDto
    {
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public string Reason { get; set; } = null!;
    }

    public class LeaveBlockResponseDto
    {
        public int LeaveID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int UserID { get; set; }
        public int? ApprovedByID { get; set; }
    }

    public class EmployeeSkillRequestDto
    {
        [Required, MaxLength(100)] public string SkillName { get; set; } = null!;
        [Required] public string ProficiencyLevel { get; set; } = null!;
        [Required] public DateTime CertifiedDate { get; set; }
        [Required] public int UserID { get; set; }
    }

    public class EmployeeSkillResponseDto
    {
        public int EmpSkillID { get; set; }
        public string SkillName { get; set; } = null!;
        public string ProficiencyLevel { get; set; } = null!;
        public DateTime CertifiedDate { get; set; }
        public string Status { get; set; } = null!;
        public int UserID { get; set; }
    }

    public class SkillRequirementDto
    {
        public int SkillReqID { get; set; }
        public string SkillName { get; set; } = null!;
        public int MinCountPerShift { get; set; }
        public string Status { get; set; } = null!;
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public string LocationName { get; set; } = "Unassigned";
        public string DepartmentName { get; set; } = "Unassigned";
    }

    public class CreateSkillRequirementDto
    {
        [Required, MaxLength(100)] public string SkillName { get; set; } = null!;
        [Required] public int MinCountPerShift { get; set; }
        [Required] public string Status { get; set; } = null!;
        [Required] public int LocationID { get; set; }
        [Required] public int DepartmentID { get; set; }
    }

    public class UpdateSkillRequirementDto
    {
        [Required, MaxLength(100)] public string SkillName { get; set; } = null!;
        [Required] public int MinCountPerShift { get; set; }
        [Required] public string Status { get; set; } = null!;
        [Required] public int LocationID { get; set; }
        [Required] public int DepartmentID { get; set; }
    }

    public class AvailabilityRequestDto
    {
        [Required] public DateTime WeekStartDate { get; set; }
        [Required, MaxLength(200)] public string AvailableDays { get; set; } = null!;
        [MaxLength(50)] public string? PreferredShiftType { get; set; }
        [Required] public decimal MaxHoursPerWeek { get; set; }
        [Required] public int UserID { get; set; }
    }

    public class AvailabilityResponseDto
    {
        public int AvailabilityID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string AvailableDays { get; set; } = null!;
        public string? PreferredShiftType { get; set; }
        public decimal MaxHoursPerWeek { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Status { get; set; } = null!;
        public int UserID { get; set; }
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

    public class EmployeeFullDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public List<AvailabilityDto> Availability { get; set; } = new List<AvailabilityDto>();
        public List<EmployeeSkillDto> Skills { get; set; } = new List<EmployeeSkillDto>();
    }

    public class EmployeeScheduleDto
    {
        public int UserId { get; set; }
        public string EmployeeName { get; set; } = "Unknown";
        public List<EmployeeShiftDto> Shifts { get; set; } = new List<EmployeeShiftDto>();
        public List<EmployeeAvailabilityDto> Availabilities { get; set; } = new List<EmployeeAvailabilityDto>();
    }

    public class EmployeeShiftDto
    {
        public int AssignmentId { get; set; }
        public DateTime AssignedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = null!;
    }

    public class EmployeeAvailabilityDto
    {
        public int AvailabilityId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string AvailableDays { get; set; } = null!;
        public string? PreferredShiftType { get; set; }
        public string Status { get; set; } = null!;
    }

    // Identical DTO models from Identity Service for mapping details
    public class UserDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class WorkLocationDto
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; } = null!;
    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
    }
}
