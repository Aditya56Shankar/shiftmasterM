using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.DTOs
{
    public class UpdateAssignmentDto
    {
        [Required(ErrorMessage = "Employee assignment target is required.")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Updated shift start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Updated shift end time is required.")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Updated shift operational role is required.")]
        [StringLength(100, ErrorMessage = "Role cannot exceed 100 characters.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "An explicit operational status update is required.")]
        public ShiftAssignmentStatus Status { get; set; }
    }
}
