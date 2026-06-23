namespace Services.DTOs
{
    public class WorkLocationDto
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; } 
        public string Type { get; set; }
        public string City { get; set; } 
        public int ManagerID { get; set; }
        public string OperatingHours { get; set; }
        public string Status { get; set; } 
    }
}