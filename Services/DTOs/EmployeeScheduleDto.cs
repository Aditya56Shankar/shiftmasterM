using System;
using System.Collections.Generic;
using System.Text;

namespace shiftMaster.Services.DTOs
{

    public class EmployeeScheduleDto
    {
        public int UserId { get; set; }

        public string EmployeeName { get; set; }

        public List<EmployeeShiftDto> Shifts { get; set; }

        public List<EmployeeAvailabilityDto> Availabilities { get; set; }
    }

    public class EmployeeShiftDto
    {
        public int AssignmentId { get; set; }

        public DateTime AssignedDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Status { get; set; }
    }

    public class EmployeeAvailabilityDto
    {
        public int AvailabilityId { get; set; }

        public DateTime WeekStartDate { get; set; }

        public string AvailableDays { get; set; }

        public string PreferredShiftType { get; set; }

        public string Status { get; set; }
    }

}
