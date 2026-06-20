namespace Services.DTOs
{
	public class CoverEligibilityDto
	{
		public int UserID { get; set; }
		public string EmployeeID { get; set; }
		public string Name { get; set; }
		public List<string> MatchingSkills { get; set; } = new();
	}
}
