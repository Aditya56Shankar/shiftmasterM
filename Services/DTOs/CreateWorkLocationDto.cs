using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs
{
    public class CreateWorkLocationDto
    {
        public string LocationName { get; set; }
        public string Type { get; set; }
        public string City { get; set; }
        public int ManagerID { get; set; }
        public string OperatingHours { get; set; }
    }
}
