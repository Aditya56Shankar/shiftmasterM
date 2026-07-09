using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

    public class TimesheetDtoResponse
    {
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public decimal TotalHours { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal PublicHolidayHours { get; set; }
        public string Status { get; set; }
        public int? SupervisorApprovedByID { get; set; }
        public int? HrApprovedByID { get; set; }
        public int? PayrollProcessedByID { get; set; }
    }

}
