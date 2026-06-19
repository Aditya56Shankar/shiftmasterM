using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

    public class AttendanceDtoResponse
    {
        public int AttendanceID { get; set; }
        public decimal ActualHoursWorked { get; set; }
        public int VarianceMinutes { get; set; }
        public AttendanceStatus Status { get; set; }
    }

}
