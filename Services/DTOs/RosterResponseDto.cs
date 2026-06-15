using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Services.DTOs
{
    public class RosterResponseDto
    {
        public int Id { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public int CreatedByID { get; set; }
        public RosterStatus Status { get; set; }
        public DateTime PublishedDate{get; set;}
    }
}