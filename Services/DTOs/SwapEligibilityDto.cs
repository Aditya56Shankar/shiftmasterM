namespace Services.DTOs
{
    public class SwapEligibilityDto
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }

        // New: all assignment options with date + assignment id
        public List<SwapAssignmentOptionDto> AvailableAssignments { get; set; } = new();
    }
}