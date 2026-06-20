namespace Services.DTOs
{
	public class SwapEligibilityDto
	{
		public int UserID { get; set; }
		public string EmployeeID { get; set; }
		public string Name { get; set; }
		public int? AvailableAssignmentID { get; set; }  // target's shift for the same roster week, if any
	}
}
