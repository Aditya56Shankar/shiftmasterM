using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Enums;

namespace Services.DTOs
{

    public class LeaveBlockRequestDto
    {
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        [EnumDataType(typeof(LeaveReason), ErrorMessage = "Invalid Leave Reason")]
        public LeaveReason Reason { get; set; }
    }
}

