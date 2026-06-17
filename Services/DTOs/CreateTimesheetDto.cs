using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{

    public class CreateTimesheetDto
    {
        public int UserID { get; set; }
        public DateTime WeekStartDate { get; set; }
    }

}
