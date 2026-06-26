using System.Collections.Generic;

namespace Services.DTOs
{
    public class EmployeeFullDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }

        public List<AvailabilityDto> Availability { get; set; }
        public List<EmployeeSkillDto> Skills { get; set; }
    }
}
