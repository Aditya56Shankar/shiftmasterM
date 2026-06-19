using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.DTOs
{
    public class CreateRosterDto
    {
        [Required(ErrorMessage = "Location ID is required.")]
        public int LocationID { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "The week's starting date is required.")]
        public DateTime WeekStartDate { get; set; }

        [Required(ErrorMessage = "Supervisor reference ID is required.")]
        public int CreatedByID { get; set; }
    }
}
