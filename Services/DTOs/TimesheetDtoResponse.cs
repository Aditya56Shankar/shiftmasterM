using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

    public class TimesheetDtoResponse
    {
        public int TimesheetID { get; set; }
        public decimal TotalHours { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public TimesheetStatus Status { get; set; }
    }

}
