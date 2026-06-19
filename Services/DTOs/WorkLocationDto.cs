namespace Services.DTOs
{
    public class WorkLocationDto
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int ManagerID { get; set; }
        public string OperatingHours { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CreateWorkLocationDto
    {
        public string LocationName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int ManagerID { get; set; }
        public string OperatingHours { get; set; } = string.Empty;
    }
}